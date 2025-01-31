using System.Collections.Generic;
using System;
using UnityEngine;
using ObservableCollections;
using R3;

using LostKaiju.Utils;
using LostKaiju.Services.Inputs;
using LostKaiju.Boilerplates.Locator;
using LostKaiju.Boilerplates.FSM;
using LostKaiju.Boilerplates.FSM.FiniteTransitions;
using LostKaiju.Game.Creatures.Features;
using LostKaiju.Game.Player.Data;
using LostKaiju.Game.Player.Data.StateParameters;
using LostKaiju.Game.Player.Behaviour.PlayerControllerStates;
using LostKaiju.Game.Creatures.Views;

namespace LostKaiju.Game.Player.Behaviour
{
    public class PlayerInputPresenter : IPlayerInputPresenter
    {
        protected ICreatureBinder _creature;
        private readonly PlayerControlsData _controlsData;
        private FiniteStateMachine _finiteStateMachine;
        private IInputProvider _inputProvider;
        private readonly List<Timer> _cooldownTimers = new(2);
        private Timer _jumpInputBufferTimer;
        private bool _readJump;
        private bool _readAttack;

        public PlayerInputPresenter(PlayerControlsData controlsData)
        {
            _controlsData = controlsData;
        }

#region CreaturePresenter
        public void Bind(ICreatureBinder creature)
        {
            _creature = creature;
            var features = creature.Features;
            var groundCheck = features.Resolve<GroundCheck>();
            var flipper = features.Resolve<Flipper>();
            var attacker = features.Resolve<IAttacker>();
            
            _inputProvider = ServiceLocator.Instance.Get<IInputProvider>();

            // idle state
            var idleState = new IdleState();

            // walk state
            var walkState = new WalkState();
            var walkParameters = _controlsData.Walk;
            walkState.Init(walkParameters, _creature.Rigidbody);
            walkState.IsPositiveDirectionX.Subscribe(flipper.LookRight);

            // jump state
            var jumpState = new JumpState();
            var jumpParameters = _controlsData.Jump;
            jumpState.Init(jumpParameters, _creature.Rigidbody);
            var jumpCooldownTimer = new Timer(jumpParameters.Cooldown, true);
            _cooldownTimers.Add(jumpCooldownTimer);
            jumpState.OnEnter.Subscribe( _ => jumpCooldownTimer.Refresh());
            _jumpInputBufferTimer = new Timer(jumpParameters.InputTimeBufferSize, true);

            // dash state (optional)
            var dashState = new DashState();
            var dashParameters = new DashParameters();
            var dashRefreshed = Observable.Merge(
                Observable.EveryValueChanged(dashState, x => x.IsCompleted.CurrentValue)
                    .Skip(1)
                    .Where(x => x == true && groundCheck.IsGrounded == true)
                    .Select(_ => true), // finished on the ground
                Observable.EveryValueChanged(groundCheck, x => x.IsGrounded)
                    .Skip(1)
                    .Where(x => x == true));  // on grounded signal
            dashState.Init(dashParameters, _creature.Rigidbody, 
                Observable.EveryValueChanged(flipper, x => x.IsLookingToTheRight),
                isRefreshed: dashRefreshed);
            var dashCooldownTimer = new Timer(dashParameters.Cooldown, true);
            _cooldownTimers.Add(dashCooldownTimer);
            dashState.OnEnter.Subscribe(_ => dashCooldownTimer.Refresh());

            // attack state
            var attackState = new AttackState();
            // var attackParameters = _controlsData.Attack;
            attackState.Init(attacker);


            BindAnimations(idleState, walkState, jumpState, attackState);

            var transitions = new IFiniteTransition[]
            {
                new SameForMultipleTransition<AttackState>(
                    () => _readAttack && attackState.IsAttackReady.CurrentValue, 
                    new Type[] { typeof(IdleState), typeof(WalkState), typeof(JumpState), typeof(DashState) }),
                new FiniteTransition<AttackState, IdleState>(() => attackState.IsAttackCompleted.CurrentValue),
                new FiniteTransition<WalkState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted && 
                    groundCheck.IsGrounded && jumpCooldownTimer.IsCompleted),
                new FiniteTransition<JumpState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<JumpState, IdleState>(() => _inputProvider.GetHorizontal == 0),
                new FiniteTransition<IdleState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<IdleState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted 
                    && groundCheck.IsGrounded && jumpCooldownTimer.IsCompleted),
                new SameForMultipleTransition<DashState>(() => _inputProvider.GetShift && 
                    dashCooldownTimer.IsCompleted && dashState.IsRefreshed.CurrentValue,
                    new Type[] { typeof(IdleState), typeof(WalkState), typeof(JumpState) }),
                new FiniteTransition<DashState, IdleState>(() => dashState.IsCompleted.CurrentValue),
                new FiniteTransition<WalkState, IdleState>(() => _inputProvider.GetHorizontal == 0) // low priority
            };

            var observableTransitions = new ObservableList<IFiniteTransition>(transitions);

            _finiteStateMachine = new BaseFiniteStateMachine();
            _finiteStateMachine.AddStates(walkState, jumpState, idleState, dashState, attackState);
            _finiteStateMachine.AddTransitions(observableTransitions);
            
            _finiteStateMachine.Init(typeof(IdleState));
        }
        
        public void UpdateLogic()
        {
            _finiteStateMachine.CurrentState.UpdateLogic();

            foreach (Timer timer in _cooldownTimers)
            {
                timer.Tick();
            }
            _jumpInputBufferTimer.Tick();

            _readJump = _inputProvider.GetJump;
            if (_readJump && _jumpInputBufferTimer.IsCompleted)
            {
                _jumpInputBufferTimer.Refresh();
            }
            _readAttack = _inputProvider.GetAttack;
        }

        public void FixedUpdateLogic()
        {
            _finiteStateMachine.CurrentState.FixedUpdateLogic();

            //if (GroundCheck.IsGrounded)
                ApplyFriction();
        }
#endregion

        private void BindAnimations(IdleState idleState, WalkState walkState, 
            JumpState jumpState, AttackState attackState)
        {
            int noFadeLayerIndex = _creature.Animator.GetLayerIndex(AnimationLayers.NO_FADE);

            walkState.OnEnter.Subscribe(_ => _creature.Animator.CrossFade(AnimationClips.WALK, 0.02f));
            jumpState.OnEnter.Subscribe(_ => _creature.Animator.CrossFadeInFixedTime(AnimationClips.IDLE, 0.2f));
            idleState.OnEnter.Subscribe(_ => {
                _creature.Animator.CrossFade(AnimationClips.IDLE, 0.2f);
                _creature.Animator.Play(AnimationClips.LOOK_AROUND, noFadeLayerIndex);
            });
            idleState.OnExit.Subscribe(_ => _creature.Animator.Play(AnimationClips.EMPTY, noFadeLayerIndex));
            attackState.OnEnter.Subscribe(_ => _creature.Animator.Play(AnimationClips.ATTACK_FORWARD));
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
    }
}
