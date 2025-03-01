using System.Collections.Generic;
using System;
using UnityEngine;
using ObservableCollections;
using R3;

using LostKaiju.Utils;
using LostKaiju.Services.Inputs;
using LostKaiju.Boilerplates.FSM;
using LostKaiju.Boilerplates.FSM.FiniteTransitions;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Player.Data;
using LostKaiju.Game.World.Player.Data.StateParameters;
using LostKaiju.Game.World.Player.Behaviour.PlayerControllerStates;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Player.Views;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public class PlayerInputPresenter : IPlayerInputPresenter, IDisposable
    {
        protected ICreatureBinder _creature;
        private readonly PlayerControlsData _controlsData;
        private FiniteStateMachine _movementFSM;
        private readonly IInputProvider _inputProvider;
        private readonly PlayerControllerState.Factory _stateFactory;
        private IGroundCheck _groundCheck;
        private PlayerJuicySystem _playerJuicySystem;
        private readonly List<Timer> _cooldownTimers = new(2);
        private Timer _jumpInputBufferTimer;
        private bool _readJump;
        private bool _isInputEnabled = true;
        private readonly CompositeDisposable _disposables = new();

        public PlayerInputPresenter(PlayerControlsData controlsData, IInputProvider inputProvider, 
            PlayerControllerState.Factory stateFactory)
        {
            _controlsData = controlsData;
            _inputProvider = inputProvider;
            _stateFactory = stateFactory;
        }

        public void SetInputEnabled(bool isEnabled)
        {
            _isInputEnabled = isEnabled;
        }

#region CreaturePresenter
        public void Bind(ICreatureBinder creature)
        {
            _creature = creature;
            var features = creature.Features;
            _groundCheck = features.Resolve<IGroundCheck>();
            var flipper = features.Resolve<IFlipper>();
            var attacker = features.Resolve<IAttacker>();
            _playerJuicySystem = features.Resolve<PlayerJuicySystem>();

            // idle state
            var idleState = new IdleState();

            // walk state
            var walkState = _stateFactory.Create<WalkState>();
            var walkParameters = _controlsData.Walk;
            walkState.Init(walkParameters, _creature.Rigidbody);
            walkState.IsPositiveDirectionX.Subscribe(flipper.LookRight);

            // jump state
            var jumpState = _stateFactory.Create<JumpState>();
            var jumpParameters = _controlsData.Jump;
            jumpState.Init(jumpParameters, _creature.Rigidbody);
            var jumpCooldownTimer = new Timer(jumpParameters.Cooldown, true);
            _cooldownTimers.Add(jumpCooldownTimer);
            jumpState.OnEnter.Subscribe( _ => jumpCooldownTimer.Refresh());
            _jumpInputBufferTimer = new Timer(jumpParameters.InputTimeBufferSize, true);

            // dash state (optional)
            var dashState = _stateFactory.Create<DashState>();
            var dashParameters = new DashParameters();
            var dashRefreshed = Observable.Merge(
                Observable.EveryValueChanged(dashState, x => x.IsCompleted.CurrentValue)
                    .Skip(1)
                    .Where(x => x == true && _groundCheck.IsGrounded == true)
                    .Select(_ => true), // finished on the ground
                Observable.EveryValueChanged(_groundCheck, x => x.IsGrounded)
                    .Skip(1)
                    .Where(x => x == true));  // on grounded signal
            dashState.Init(dashParameters, _creature.Rigidbody, 
                Observable.EveryValueChanged(flipper, x => x.IsLookingToTheRight),
                isRefreshed: dashRefreshed);
            var dashCooldownTimer = new Timer(dashParameters.Cooldown, true);
            _cooldownTimers.Add(dashCooldownTimer);
            dashState.OnEnter.Subscribe(_ => dashCooldownTimer.Refresh());

            // attack state
            var attackState = _stateFactory.Create<AttackState>();
            // var attackParameters = _controlsData.Attack;
            attackState.Init(attacker);


            BindAnimations(idleState, walkState, jumpState, attackState);

            var transitions = new IFiniteTransition[]
            {
                new SameForMultipleTransition<AttackState>(
                    () => _inputProvider.GetAttack && attackState.IsAttackReady.CurrentValue, 
                    new Type[] { typeof(IdleState), typeof(WalkState), typeof(JumpState), typeof(DashState) }),
                new FiniteTransition<AttackState, IdleState>(() => true),
                new FiniteTransition<WalkState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted && 
                    _groundCheck.IsGrounded && jumpCooldownTimer.IsCompleted),
                new FiniteTransition<JumpState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<JumpState, IdleState>(() => _inputProvider.GetHorizontal == 0),
                new FiniteTransition<IdleState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<IdleState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted 
                    && _groundCheck.IsGrounded && jumpCooldownTimer.IsCompleted),
                new SameForMultipleTransition<DashState>(() => _inputProvider.GetShift && 
                    dashCooldownTimer.IsCompleted && dashState.IsRefreshed.CurrentValue,
                    new Type[] { typeof(IdleState), typeof(WalkState), typeof(JumpState) }),
                new FiniteTransition<DashState, IdleState>(() => dashState.IsCompleted.CurrentValue),
                new FiniteTransition<WalkState, IdleState>(() => _inputProvider.GetHorizontal == 0) // low priority
            };

            var observableTransitions = new ObservableList<IFiniteTransition>(transitions);

            _movementFSM = new FiniteStateMachine();
            _movementFSM.AddStates(walkState, jumpState, idleState, dashState, attackState);
            _movementFSM.AddTransitions(observableTransitions);
            
            _movementFSM.Init(typeof(IdleState));

            _creature.OnDispose.Take(1).Subscribe(_ => Dispose());
        }
        
        public void UpdateLogic()
        {
            if (_isInputEnabled)
            {
                _movementFSM.CurrentState.UpdateLogic();
                    _readJump = _inputProvider.GetJump;
                if (_readJump)
                    _jumpInputBufferTimer.Refresh();
            }

            foreach (Timer timer in _cooldownTimers)
            {
                timer.Tick();
            }
            _jumpInputBufferTimer.Tick();
        }

        public void FixedUpdateLogic()
        {
            _movementFSM.CurrentState.FixedUpdateLogic();

            //if (GroundCheck.IsGrounded)
                ApplyFriction();
        }
#endregion

        private void BindAnimations(IdleState idleState, WalkState walkState, 
            JumpState jumpState, AttackState attackState)
        {
            var animator = _creature.Animator;
            var baseLayer = animator.GetLayerIndex(AnimationLayers.BASE);
            var noFadeLayerIndex = animator.GetLayerIndex(AnimationLayers.NO_FADE);
            var movementOverrideLayer = animator.GetLayerIndex(AnimationLayers.MOVEMENT_OVERRIDE_LAYER);
            var attackOverrideLayer = animator.GetLayerIndex(AnimationLayers.ATTACK_OVERRIDE_LAYER);

            walkState.OnEnter.Subscribe(_ => animator.CrossFade(AnimationClips.WALK, 0.02f));
            
            jumpState.OnEnter.Subscribe(_ => animator.CrossFadeInFixedTime(AnimationClips.IDLE, 0.2f));
            Observable.EveryValueChanged(_groundCheck, x => _groundCheck.IsGrounded)
                .Where(x => x == true)
                .Subscribe(_ => {
                    animator.Play(AnimationClips.EMPTY, movementOverrideLayer);
                    _playerJuicySystem.PlayStep();
                })
                .AddTo(_disposables);

            Observable.EveryValueChanged(_creature.Rigidbody, s => s.linearVelocityY)
                .Where(_ => _groundCheck.IsGrounded == false)
                .Subscribe(x => {
                    if (x > 0)
                        animator.Play(AnimationClips.AIR_UP, movementOverrideLayer);
                    else
                       animator.Play(AnimationClips.AIR_DOWN, movementOverrideLayer);
                })
                .AddTo(_disposables);

            idleState.OnEnter.Subscribe(_ => {
                var currentClipHash = animator.GetCurrentAnimatorStateInfo(baseLayer).shortNameHash;

                if (currentClipHash == AnimationClips.SITTING || currentClipHash == AnimationClips.LYING)
                {
                    animator.Play(AnimationClips.IDLE);
                }
                else
                {
                    animator.CrossFade(AnimationClips.IDLE, 0.2f);
                }

                var subscription = new SerialDisposable();

                subscription.Disposable = Observable.Timer(TimeSpan.FromSeconds(4))
                    .TakeUntil(idleState.OnExit)
                    .Subscribe(_ => {
                        animator.CrossFadeInFixedTime(AnimationClips.SITTING, 0.5f);

                        subscription.Disposable = Observable.Timer(TimeSpan.FromSeconds(4))
                            .TakeUntil(idleState.OnExit)
                            .Subscribe(_ => {
                                animator.CrossFadeInFixedTime(AnimationClips.LYING, 0.5f);
                                animator.Play(AnimationClips.LYING_SCALES, noFadeLayerIndex);
                            }).AddTo(_disposables);
                    }).AddTo(_disposables);

                idleState.OnExit.Take(1).Subscribe(_ => subscription.Dispose());
                
                animator.Play(AnimationClips.LOOK_AROUND, noFadeLayerIndex);
            });

            idleState.OnExit.Subscribe(_ => animator.Play(AnimationClips.EMPTY, noFadeLayerIndex));
            attackState.OnEnter.Subscribe(_ => animator.Play(AnimationClips.ATTACK_FORWARD, attackOverrideLayer));
            attackState.IsAttackCompleted.Subscribe(x => animator.Play(AnimationClips.EMPTY, attackOverrideLayer));
        }

        private void ApplyFriction()
        {
            if (Mathf.Abs(_creature.Rigidbody.linearVelocityX) > 0.01f)
            {
                float frictionForce = Mathf.Min(Mathf.Abs(_creature.Rigidbody.linearVelocityX), _controlsData.Walk.FrictionForce);

                _creature.Rigidbody.AddForceX(frictionForce * -Mathf.Sign(_creature.Rigidbody.linearVelocityX), 
                    ForceMode2D.Impulse);
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
