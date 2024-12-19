using UnityEngine;
using ObservableCollections;
using R3;

using LostKaiju.Architecture.Utils;
using LostKaiju.Architecture.Providers.Inputs;
using LostKaiju.Architecture.Services;
using LostKaiju.Architecture.FSM;
using LostKaiju.Architecture.FSM.FiniteTransitions;
using LostKaiju.Creatures.CreatureFeatures;
using LostKaiju.Creatures.Presenters;
using LostKaiju.Player.Data;
using LostKaiju.Player.Data.StateParameters;
using LostKaiju.Player.Behaviour.PlayerControllerStates;
using LostKaiju.Creatures.Views;

namespace LostKaiju.Player.Behaviour
{
    public class PlayerPresenter : CreaturePresenter
    {
        private readonly PlayerControlsData _data;
        private FiniteStateMachine _finiteStateMachine;
        private float _jumpInputBufferedTime;
        private float _waitToJump;
        private float _waitToDash;
        private bool _readJump;
        private IInputProvider _inputProvider;

        public PlayerPresenter(PlayerControlsData data)
        {
            _data = data;
        }

        public override void Bind(CreatureBinder creature, Holder<ICreatureFeature> features)
        {
            base.Bind(creature, features);

            var groundCheck = features.Resolve<GroundCheck>();
            var flipper = features.Resolve<Flipper>();

            _inputProvider = ServiceLocator.Current.Get<IInputProvider>();

            var walkParameters = _data.Walk;
            walkParameters.WalkRigidbody = Creature.Rigidbody;

            var walkState = new WalkState();
            walkState.OnEnter.Subscribe(_ => Creature.Animator.CrossFade(AnimationClips.WALK, 0.02f));
            walkState.Init(walkParameters, Creature.Rigidbody); // () => groundCheck.IsGrounded
            walkState.IsPositiveDirectionX.Subscribe(flipper.LookRight);

            var jumpParameters = _data.Jump;
            jumpParameters.JumpRigidbody = Creature.Rigidbody;
            
            var jumpState = new JumpState();
            jumpState.Init(jumpParameters, Creature.Rigidbody);
            jumpState.OnEnter.Subscribe( _ => {
                _waitToJump = _data.Jump.Cooldown;
                Creature.Animator.CrossFadeInFixedTime(AnimationClips.IDLE, 0.2f);
            });

            var idleState = new IdleState();
            idleState.OnEnter.Subscribe(_ => Creature.Animator.CrossFade(AnimationClips.IDLE, 0.2f));

            var dashParameters = new DashParameters
            {
                rigidBody = Creature.Rigidbody
            };

            var dashState = new DashState();
            dashState.Init(dashParameters, Creature.Rigidbody, Observable.EveryValueChanged(flipper, x => x.IsLooksToTheRight));
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
                _jumpInputBufferedTime = _data.Jump.InputTimeBufferSize;
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
            if (Mathf.Abs(Creature.Rigidbody.linearVelocityX) > 0.01f)
            {
                float frictionForce = Mathf.Min(Mathf.Abs(Creature.Rigidbody.linearVelocityX), _data.Walk.FrictionForce);

                Creature.Rigidbody.AddForceX(frictionForce * -Mathf.Sign(Creature.Rigidbody.linearVelocityX), 
                    ForceMode2D.Impulse);
            }
        }
    }
}
