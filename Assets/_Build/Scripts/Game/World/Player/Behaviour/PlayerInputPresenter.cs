using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using R3;

using LostKaiju.Utils;
using LostKaiju.Services.Inputs;
using LostKaiju.Boilerplates.FSM;
using LostKaiju.Boilerplates.FSM.FiniteTransitions;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Player.Data;
using LostKaiju.Game.World.Player.Behaviour.PlayerControllerStates;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Player.Views;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public class PlayerInputPresenter : IPlayerInputPresenter
    {
        protected ICreatureBinder _creature;
        private readonly PlayerControlsData _controlsData;
        private FiniteStateMachine _finiteStateMachine;
        private IInputProvider _inputProvider;
        private readonly IInputProvider _cachedInputProvider;
        private IGroundCheck _groundCheck;
        private IDamageReceiver _damageReceiver;
        private PlayerJuicySystem _playerJuicySystem;
        private readonly List<Timer> _cooldownTimers = new(3);
        private Timer _jumpInputBufferTimer;
        private bool _readJump;
        private readonly CompositeDisposable _disposables = new();

        public PlayerInputPresenter(PlayerControlsData controlsData, IInputProvider inputProvider)
        {
            _controlsData = controlsData;
            _inputProvider = _cachedInputProvider = inputProvider;
        }

        public void SetInputEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                _inputProvider = _cachedInputProvider;
            }
            else 
                _inputProvider = new BlockedInputProvider();
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
            attacker.OnHitPositionSent.Subscribe(x => _playerJuicySystem.PlayHit(x));
            // idle state
            var idleState = new IdleState();

            _damageReceiver = features.Resolve<IDamageReceiver>();
            _damageReceiver.OnDamageTaken.Where(_ => _finiteStateMachine.CurrentState == idleState)
                .Subscribe(_ => _finiteStateMachine.ForceState(typeof(IdleState)));
            

            // walk state
            var walkParameters = _controlsData.Walk;
            var walkState = new WalkState(walkParameters, _creature.Rigidbody, () => _inputProvider.GetHorizontal);
            walkState.IsPositiveDirectionX.Subscribe(flipper.LookRight);

            // jump state
            var jumpParameters = _controlsData.Jump;
            var jumpState = new JumpState(jumpParameters, _creature.Rigidbody);
            var jumpCooldownTimer = new Timer(jumpParameters.Cooldown, true);
            _cooldownTimers.Add(jumpCooldownTimer);
            jumpState.OnEnter.Subscribe( _ => jumpCooldownTimer.Refresh());
            _jumpInputBufferTimer = new Timer(jumpParameters.InputTimeBufferSize, true);

            // dash state (optional)
            // var dashParameters = new DashParameters();
            // var dashState = new DashState();
            // var dashRefreshed = Observable.Merge(
            //     Observable.EveryValueChanged(dashState, x => x.IsCompleted.CurrentValue)
            //         .Skip(1)
            //         .Where(x => x == true && _groundCheck.IsGrounded == true)
            //         .Select(_ => true), // finished on the ground
            //     Observable.EveryValueChanged(_groundCheck, x => x.IsGrounded)
            //         .Skip(1)
            //         .Where(x => x == true));  // on grounded signal
            // dashState.Init(dashParameters, _creature.Rigidbody, 
            //     Observable.EveryValueChanged(flipper, x => x.IsLookingToTheRight),
            //     isRefreshed: dashRefreshed);
            // var dashCooldownTimer = new Timer(dashParameters.Cooldown, true);
            // _cooldownTimers.Add(dashCooldownTimer);
            // dashState.OnEnter.Subscribe(_ => dashCooldownTimer.Refresh());

            // attack state
            // var attackParameters = _controlsData.Attack;
            var attackState = new AttackState(attacker);
            var attackCooldownTimer = new Timer(_controlsData.Attack.Cooldown, true);
            _cooldownTimers.Add(attackCooldownTimer);
            attackState.IsAttackCompleted.Where(x => x == true).Subscribe(_ => attackCooldownTimer.Refresh());

            BindAnimations(idleState, walkState, jumpState, attackState);

            var transitions = new IFiniteTransition[]
            {
                new SameForMultipleTransition<AttackState>(
                    () => _inputProvider.GetAttack && attackState.IsAttackReady.CurrentValue &&  // checks Canvas UI only
                        attackCooldownTimer.IsCompleted,
                    new Type[] { typeof(IdleState), typeof(WalkState), typeof(JumpState), typeof(DashState) }),
                new FiniteTransition<AttackState, IdleState>(() => true),
                new FiniteTransition<WalkState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted && 
                    _groundCheck.IsGrounded && jumpCooldownTimer.IsCompleted),
                new FiniteTransition<JumpState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<JumpState, IdleState>(() => _inputProvider.GetHorizontal == 0),
                new FiniteTransition<IdleState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<IdleState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted 
                    && _groundCheck.IsGrounded && jumpCooldownTimer.IsCompleted),
                // new SameForMultipleTransition<DashState>(() => _inputProvider.GetShift && 
                //     dashCooldownTimer.IsCompleted && dashState.IsRefreshed.CurrentValue,
                //     new Type[] { typeof(IdleState), typeof(WalkState), typeof(JumpState) }),
                // new FiniteTransition<DashState, IdleState>(() => dashState.IsCompleted.CurrentValue),
                new FiniteTransition<WalkState, IdleState>(() => _inputProvider.GetHorizontal == 0) // low priority
            };

            _finiteStateMachine = new FiniteStateMachine();
            _finiteStateMachine.AddStates(walkState, jumpState, idleState, attackState);
            // _finiteStateMachine.AddState(dashState);
            _finiteStateMachine.AddTransitions(transitions);
            
            _finiteStateMachine.Init(typeof(IdleState));

            _creature.OnDispose.Take(1).Subscribe(_ => Dispose());
        }
        
        public void UpdateLogic()
        {
            _finiteStateMachine.CurrentState?.UpdateLogic();
            _readJump = _inputProvider.GetJump;
            if (_readJump)
                _jumpInputBufferTimer.Refresh();
            
            foreach (Timer timer in _cooldownTimers)
            {
                timer.Tick();
            }
            _jumpInputBufferTimer.Tick();
        }

        public void FixedUpdateLogic()
        {
            _finiteStateMachine.CurrentState.FixedUpdateLogic();

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

            walkState.OnEnter.Subscribe(_ => animator.CrossFade(AnimationClips.WALK, 0.02f)).AddTo(_disposables);
            
            jumpState.OnEnter.Subscribe(_ => animator.CrossFadeInFixedTime(AnimationClips.IDLE, 0.2f)).AddTo(_disposables);
            Observable.EveryValueChanged(_groundCheck, x => _groundCheck.IsGrounded)
                .SkipFrame(1)
                .Where(x => x == true)
                .Subscribe(_ => {
                    animator.Play(AnimationClips.EMPTY, movementOverrideLayer);
                    _playerJuicySystem.PlayStep();
                })
                .AddTo(_disposables);

            Observable.EveryValueChanged(_creature.Rigidbody, s => s.linearVelocityY)
                .SkipFrame(1)
                .Where(_ => _groundCheck.IsGrounded == false)
                .Subscribe(x => 
                {
                    if (x > 0)
                        animator.Play(AnimationClips.AIR_UP, movementOverrideLayer);
                    else
                       animator.Play(AnimationClips.AIR_DOWN, movementOverrideLayer);
                })
                .AddTo(_disposables);

            idleState.OnEnter.Subscribe(_ => 
            {
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
                    .Subscribe(_ => 
                    {
                        animator.CrossFadeInFixedTime(AnimationClips.SITTING, 0.5f);

                        subscription.Disposable = Observable.Timer(TimeSpan.FromSeconds(4))
                            .TakeUntil(idleState.OnExit)
                            .Subscribe(_ => 
                            {
                                animator.CrossFadeInFixedTime(AnimationClips.LYING, 0.5f);
                                animator.Play(AnimationClips.LYING_SCALES, noFadeLayerIndex);
                            }).AddTo(_disposables);
                    }).AddTo(_disposables);

                animator.Play(AnimationClips.LOOK_AROUND, noFadeLayerIndex);
            }).AddTo(_disposables);

            idleState.OnExit.Subscribe(_ => animator.Play(AnimationClips.EMPTY, noFadeLayerIndex)).AddTo(_disposables);
            attackState.OnEnter.Subscribe(_ => animator.Play(AnimationClips.ATTACK_FORWARD, attackOverrideLayer)).AddTo(_disposables);
            attackState.IsAttackCompleted.Where(x => x == true)
                .Subscribe(x => animator.Play(AnimationClips.EMPTY, attackOverrideLayer)).AddTo(_disposables);
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
            _finiteStateMachine.Dispose();
        }
    }
}
