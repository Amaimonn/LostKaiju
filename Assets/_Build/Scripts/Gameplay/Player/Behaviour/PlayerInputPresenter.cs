using UnityEngine;
using ObservableCollections;
using R3;

using LostKaiju.Utils;
using LostKaiju.Infrastructure.Providers.Inputs;
using LostKaiju.Models.Locator;
using LostKaiju.Models.FSM;
using LostKaiju.Models.FSM.FiniteTransitions;
using LostKaiju.Gameplay.Creatures.Features;
using LostKaiju.Gameplay.Creatures.Presenters;
using LostKaiju.Gameplay.Player.Data;
using LostKaiju.Gameplay.Player.Data.StateParameters;
using LostKaiju.Gameplay.Player.Behaviour.PlayerControllerStates;
using LostKaiju.Gameplay.Creatures.Views;
using System.Collections.Generic;

namespace LostKaiju.Gameplay.Player.Behaviour
{
    public class PlayerInputPresenter : CreaturePresenter
    {
        private readonly PlayerControlsData _controlsData;
        private FiniteStateMachine _finiteStateMachine;
        private IInputProvider _inputProvider;
        private readonly List<Timer> _cooldownTimers = new(2);
        private Timer _jumpInputBufferTimer;
        private bool _readJump;

        public PlayerInputPresenter(PlayerControlsData controlsData)
        {
            _controlsData = controlsData;
        }

#region CreaturePresenter
        public override void Bind(CreatureBinder creature, Holder<ICreatureFeature> features)
        {
            base.Bind(creature, features);

            var groundCheck = features.Resolve<GroundCheck>();
            var flipper = features.Resolve<Flipper>();
            
            _inputProvider = ServiceLocator.Current.Get<IInputProvider>();

            // idle state
            var idleState = new IdleState();

            // walk state
            var walkState = new WalkState();
            var walkParameters = _controlsData.Walk;
            walkState.Init(walkParameters, Creature.Rigidbody);
            walkState.IsPositiveDirectionX.Subscribe(flipper.LookRight);

            // jump state
            var jumpState = new JumpState();
            var jumpParameters = _controlsData.Jump;
            jumpState.Init(jumpParameters, Creature.Rigidbody);
            var jumpCooldownTimer = new Timer(_controlsData.Jump.Cooldown, true);
            _cooldownTimers.Add(jumpCooldownTimer);
            jumpState.OnEnter.Subscribe( _ => jumpCooldownTimer.Refresh());
            _jumpInputBufferTimer = new Timer(_controlsData.Jump.InputTimeBufferSize, true);

            // dash state (optional)
            var dashState = new DashState();
            var dashParameters = new DashParameters();
            dashState.Init(dashParameters, Creature.Rigidbody, Observable.EveryValueChanged(flipper, x => x.IsLooksToTheRight));
            var dashCooldownTimer = new Timer(dashParameters.Cooldown, true);
            _cooldownTimers.Add(dashCooldownTimer);
            dashState.OnEnter.Subscribe(_ => dashCooldownTimer.Refresh());

            BindAnimations(idleState, walkState, jumpState);

            var transitions = new IFiniteTransition[]
            {
                new FiniteTransition<WalkState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted && groundCheck.IsGrounded 
                    && jumpCooldownTimer.IsCompleted),
                new FiniteTransition<JumpState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<JumpState, IdleState>(() => _inputProvider.GetHorizontal == 0),
                new FiniteTransition<IdleState, WalkState>(() => _inputProvider.GetHorizontal != 0),
                new FiniteTransition<IdleState, JumpState>(() => !_jumpInputBufferTimer.IsCompleted && groundCheck.IsGrounded
                    && jumpCooldownTimer.IsCompleted),
                new FiniteTransition<WalkState, DashState>(() => _inputProvider.GetShift && dashCooldownTimer.IsCompleted),
                new FiniteTransition<DashState, IdleState>(() => dashState.IsCompleted.CurrentValue),
                new FiniteTransition<IdleState, DashState>(() => _inputProvider.GetShift && dashCooldownTimer.IsCompleted),
                new FiniteTransition<JumpState, DashState>(() => _inputProvider.GetShift && dashCooldownTimer.IsCompleted),
                new FiniteTransition<WalkState, IdleState>(() => _inputProvider.GetHorizontal == 0) // low priority
            };

            var observableTransitions = new ObservableList<IFiniteTransition>(transitions);

            _finiteStateMachine = new BaseFiniteStateMachine(typeof(IdleState));
            _finiteStateMachine.AddStates(walkState, jumpState, idleState, dashState);
            _finiteStateMachine.AddTransitions(observableTransitions);
            
            _finiteStateMachine.Init();
        }
        
        public override void UpdateLogic()
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
        }

        public override void FixedUpdateLogic()
        {
            _finiteStateMachine.CurrentState.FixedUpdateLogic();

            //if (GroundCheck.IsGrounded)
                ApplyFriction();
        }
#endregion

        private void BindAnimations(IdleState idleState, WalkState walkState, JumpState jumpState)
        {
            int noFadeLayerIndex = Creature.Animator.GetLayerIndex(AnimationLayers.NO_FADE);

            walkState.OnEnter.Subscribe(_ => Creature.Animator.CrossFade(AnimationClips.WALK, 0.02f));
            jumpState.OnEnter.Subscribe(_ => Creature.Animator.CrossFadeInFixedTime(AnimationClips.IDLE, 0.2f));
            idleState.OnEnter.Subscribe(_ => {
                Creature.Animator.CrossFade(AnimationClips.IDLE, 0.2f);
                Creature.Animator.Play(AnimationClips.LOOK_AROUND, noFadeLayerIndex);
            });
            idleState.OnExit.Subscribe(_ => Creature.Animator.Play(AnimationClips.EMPTY, noFadeLayerIndex));
        }

        private void ApplyFriction()
        {
            if (Mathf.Abs(Creature.Rigidbody.linearVelocityX) > 0.01f)
            {
                float frictionForce = Mathf.Min(Mathf.Abs(Creature.Rigidbody.linearVelocityX), _controlsData.Walk.FrictionForce);

                Creature.Rigidbody.AddForceX(frictionForce * -Mathf.Sign(Creature.Rigidbody.linearVelocityX), 
                    ForceMode2D.Impulse);
            }
        }
    }
}
