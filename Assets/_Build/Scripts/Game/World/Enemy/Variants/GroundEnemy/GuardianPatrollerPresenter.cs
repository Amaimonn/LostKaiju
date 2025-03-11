using System;
using UnityEngine;
using R3;

using LostKaiju.Utils;
using LostKaiju.Boilerplates.FSM;
using LostKaiju.Boilerplates.FSM.FiniteTransitions;
using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Agents.Sensors;
using LostKaiju.Game.World.Enemy.Configs;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.StateParameters;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.UI.MVVM.Gameplay.EnemyCreature;

namespace LostKaiju.Game.World.Enemy
{
    public class GuardianPatrollerPresenter : MonoBehaviour
    {
        [SerializeField] private CreatureBinder _creatureBinder;
        [SerializeField] private GroundAgent _groundAgent;
        [SerializeField] private OccludablePlayerSensor _playerSensor;
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private float _attackDelay = 0.3f;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _attackDistance = 5;
        [SerializeField] private EnemyHealthWorldView _healthView;
        [SerializeField] private EnemyDefenceDataSO _defenceDataSO;

        private FiniteStateMachine _finiteStateMachine;
        private ITargeter _targeter;
        private Timer _attackDelayTimer;
        private Timer _attackCooldownTimer;
        private SerialDisposable _targetLossDisposable = new();

        private void Start()
        {
            _creatureBinder.Init();
            BindMovement();
            BindDefence();
        }

        private void BindMovement()
        {
            _targeter = _creatureBinder.Features.Resolve<ITargeter>();
            var attacker = _creatureBinder.Features.Resolve<IAttacker>();

            _attackDelayTimer = new Timer(_attackDelay, true);
            _attackCooldownTimer = new Timer(_attackCooldown, true);

            _playerSensor.Detected.Skip(1)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    if (!_targeter.IsTargeting)
                        _attackDelayTimer.Refresh();
                    _targeter.SetTarget(x.transform);
                    _targetLossDisposable.Disposable = Disposable.Empty;
                })
                .AddTo(this);

            _playerSensor.Detected.Skip(1)
                .Where(x => x == null)
                .Subscribe(t => 
                {
                    _targetLossDisposable.Disposable = Observable.Timer(TimeSpan.FromSeconds(1))
                        .Subscribe(_ => _targeter.SetTarget(null))
                        .AddTo(this);
                })
                .AddTo(this);

            var patrolState = new PatrolState(_groundAgent, _patrolPoints);
            patrolState.Init(new PatrolParameters());

            var chaseState = new ChaseState(_groundAgent, _targeter);
            chaseState.Init(_attackDistance);

            var attackState = new AttackState(_groundAgent, attacker);
            
            attackState.OnEnter.Subscribe(x =>
            {
                _attackCooldownTimer.Refresh();
            });

            var transitions = new IFiniteTransition[]
            {
                new SameForMultipleTransition<AttackState>(() => _targeter.IsTargeting == true && 
                _attackCooldownTimer.IsCompleted && _attackDelayTimer.IsCompleted,
                    new Type[]{typeof(ChaseState), typeof(PatrolState)}),
                new FiniteTransition<AttackState, ChaseState>(() => _targeter.IsTargeting == true),
                new FiniteTransition<AttackState, PatrolState>(() => _targeter.IsTargeting == false),
                new FiniteTransition<PatrolState, ChaseState>(() => _targeter.IsTargeting && !_attackCooldownTimer.IsCompleted),
                new FiniteTransition<ChaseState, PatrolState>(() => _targeter.IsTargeting == false)
            };

            _finiteStateMachine = new FiniteStateMachine();
            _finiteStateMachine.AddStates(patrolState, chaseState, attackState);
            _finiteStateMachine.AddTransitions(transitions);
            _finiteStateMachine.Init(typeof(PatrolState));
        }

        private void BindDefence()
        {
            var healthState = new HealthState(_defenceDataSO.MaxHealth);
            var healthModel = new HealthModel(healthState);
            var defencePresenter = new EnemyDefencePresenter(healthModel, _defenceDataSO);
            defencePresenter.Bind(_creatureBinder);
            defencePresenter.OnDeath.Take(1).Subscribe(_ => OnDeath());
            var healthViewModel = new HealthViewModel(healthModel);
            _healthView.Bind(healthViewModel);
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            _finiteStateMachine.CurrentState.UpdateLogic();

            _attackDelayTimer.Tick();
            _attackCooldownTimer.Tick();
        }
    }
}
