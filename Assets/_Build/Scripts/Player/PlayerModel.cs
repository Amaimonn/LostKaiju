using UnityEngine;
using ObservableCollections;
using R3;
using System;

public class PlayerModel : CharacterModel
{
    private readonly PlayerControlsData _data;
    private FiniteStateMachine _finiteStateMachine;
    //private ReactiveProperty<bool> _isGrounded = new();
    protected Func<bool> _checkIsGrounded;
    private float _jumpInputBufferedTime;
    private float _waitToJump;
    private float _waitToDash;
    private bool _readJump;
    private IInputProvider _inputProvider;

    public PlayerModel(PlayerControlsData data)
    {
        _data = data;
    }

    public override void Bind(IPlayer player, Holder<IEntityFeature> features)
    {
        base.Bind(player, features);
        var groundCheck = features.Resolve<GroundCheck>();
        var flipper = features.Resolve<Flipper>();
        _inputProvider = ServiceLocator.Current.Get<IInputProvider>();

        var walkParameters = _data.Walk;
        walkParameters.WalkRigidbody = ModelRigidbody;

        var walkState = new WalkState();
        walkState.OnEnter.Subscribe(_ => ModelAnimator.CrossFade(AnimationClips.WALK, 0.02f));
        walkState.Init(walkParameters, () => groundCheck.IsGrounded);
        walkState.IsPositiveDirectionX.Subscribe(flipper.LookRight);

        var jumpParameters = _data.Jump;
        jumpParameters.JumpRigidbody = ModelRigidbody;
        
        var jumpState = new JumpState();
        jumpState.Init(jumpParameters);
        jumpState.OnEnter.Subscribe( _ => {
            _waitToJump = _data.JumpCooldown;
            ModelAnimator.CrossFadeInFixedTime(AnimationClips.IDLE, 0.2f);
        });

        var idleState = new IdleState();
        idleState.OnEnter.Subscribe(_ => ModelAnimator.CrossFade(AnimationClips.IDLE, 0.2f));

        var dashParameters = new DashParameters
        {
            rigidBody = ModelRigidbody
        };

        var dashState = new DashState();
        dashState.Init(dashParameters, Observable.EveryValueChanged(flipper, x => x.IsLooksToTheRight));
        dashState.OnEnter.Subscribe(_ => _waitToDash = dashParameters.Cooldown);
        //var states = new FiniteState[] {walkState, jumpState, idleState, dashState};

        var transitions = new IFiniteTransition[]
        {
            new FiniteTransition<WalkState, JumpState>(() => _jumpInputBufferedTime > 0 && groundCheck.IsGrounded && _waitToJump <= 0),
            new FiniteTransition<JumpState, WalkState>(() => _inputProvider.GetHorizontal != 0),
            new FiniteTransition<JumpState, IdleState>(() => _inputProvider.GetHorizontal == 0),
            new FiniteTransition<IdleState, WalkState>(() => _inputProvider.GetHorizontal != 0),
            new FiniteTransition<IdleState, JumpState>(() => _jumpInputBufferedTime > 0 && groundCheck.IsGrounded && _waitToJump <= 0),
            new FiniteTransition<WalkState, DashState>(() => _inputProvider.GetShift && _waitToDash <= 0),
            new FiniteTransition<DashState, IdleState>(() => dashState.IsCompleted.CurrentValue),
            new FiniteTransition<IdleState, DashState>(() => _inputProvider.GetShift && _waitToDash <= 0),
            new FiniteTransition<JumpState, DashState>(() => _inputProvider.GetShift && _waitToDash <= 0),
            new FiniteTransition<WalkState, IdleState>(() => _inputProvider.GetHorizontal == 0) // low priority
        };

        var observableTransitions = new ObservableList<IFiniteTransition>(transitions);

        _finiteStateMachine = new BaseFiniteStateMachine(typeof(IdleState));
        // _finiteStateMachine.SetTransitionsWithStates(observableTransitions, states);
        _finiteStateMachine.AddState(walkState);
        _finiteStateMachine.AddState(jumpState);
        _finiteStateMachine.AddState(idleState);
        _finiteStateMachine.AddState(dashState);
        _finiteStateMachine.AddTransitions(observableTransitions);
        
        _finiteStateMachine.Init();
    }
    
    public override void UpdateLogic()
    {
        _finiteStateMachine.CurrentState.UpdateLogic();
        var eclapsedFrameTime = Time.deltaTime;
        if (_jumpInputBufferedTime > 0)
        {
            _jumpInputBufferedTime -= eclapsedFrameTime;
        }

        if (_waitToJump > 0)
        {
            _waitToJump -= eclapsedFrameTime;
        }

        if (_waitToDash > 0)
        {
            _waitToDash -= eclapsedFrameTime;
        }

        _readJump = _inputProvider.GetJump;
        if (_readJump)
        {
            _jumpInputBufferedTime = _data.JumpInputTimeBufferSize;
        }
    }

    public override void FixedUpdateLogic()
    {
        _finiteStateMachine.CurrentState.FixedUpdateLogic();

        //if (GroundCheck.IsGrounded)
            ApplyFriction();
    }

    private void ApplyFriction()
    {
        if (Mathf.Abs(ModelRigidbody.linearVelocityX) > 0.01f)
        {
            float frictionForce = Mathf.Min(Mathf.Abs(ModelRigidbody.linearVelocityX), _data.Walk.FrictionKoefficient);
            ModelRigidbody.AddForceX(frictionForce * -Mathf.Sign(ModelRigidbody.linearVelocityX), ForceMode2D.Impulse);
        }
    }
}