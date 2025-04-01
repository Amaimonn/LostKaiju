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
using LostKaiju.Game.World.Creatures.Presenters;

namespace LostKaiju.Game.World.Enemy
{
    public class PatrollerAIPresenter : ICreaturePresenter, IDisposable, IUpdatablePresenter
    {
#region Constructor
        private readonly GroundAgent _groundAgent;
        private readonly OccludablePlayerSensor _playerSensor;
        private readonly Transform[] _patrolPoints;
        private readonly EnemyAttackDataSO _attackData;
#endregion

        private ICreatureBinder _creature;
        private FiniteStateMachine _finiteStateMachine;
        private ITargeter _targeter;
        private Timer _attackDelayTimer;
        private Timer _attackCooldownTimer;
        private readonly SerialDisposable _targetLossDisposable = new();
        private readonly CompositeDisposable _disposables = new();

        public PatrollerAIPresenter(EnemyAttackDataSO attackData, GroundAgent groundAgent, 
            OccludablePlayerSensor playerSensor, Transform[] patrolPoints)
        {
            _attackData = attackData;
            _groundAgent = groundAgent;
            _playerSensor = playerSensor;
            _patrolPoints = patrolPoints;
        }

        public void Bind(ICreatureBinder creature)
        {
            _creature = creature;
            _targeter = _creature.Features.Resolve<ITargeter>();
            var attacker = _creature.Features.Resolve<IAttacker>();
            var pusher = _creature.Features.Resolve<IPusher>();
            var juicySystem = _creature.Features.Resolve<EnemyJuicySystem>();

            pusher.OnPushed.Subscribe(_ => _attackDelayTimer.Refresh())
                .AddTo(_disposables);

            _attackDelayTimer = new Timer(_attackData.AttackDelay, true);
            _attackCooldownTimer = new Timer(_attackData.AttackCooldown, true);

            _playerSensor.Detected.Skip(1)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    if (!_targeter.IsTargeting)
                        _attackDelayTimer.Refresh();
                    _targeter.SetTarget(x.transform);
                    _targetLossDisposable.Disposable = Disposable.Empty;
                })
                .AddTo(_disposables);

            _playerSensor.Detected.Skip(1)
                .Where(x => x == null)
                .Subscribe(t => 
                {
                    _targetLossDisposable.Disposable = Observable.Timer(TimeSpan.FromSeconds(1))
                        .Subscribe(_ => _targeter.SetTarget(null))
                        .AddTo(_disposables);
                })
                .AddTo(_disposables);

            var patrolState = new PatrolState(_groundAgent, _patrolPoints);
            patrolState.Init(new PatrolParameters());

            var chaseState = new ChaseState(_groundAgent, _targeter);
            chaseState.Init(_attackData.AttackDistance);

            var attackState = new AttackState(_groundAgent, attacker);
            
            attackState.OnEnter.Subscribe(x =>
            {
                juicySystem.PlayAttack();
                _attackCooldownTimer.Refresh();
            });

            var transitions = new IFiniteTransition[]
            {
                new SameForMultipleTransition<AttackState>(() => _targeter.IsTargeting && 
                _attackCooldownTimer.IsCompleted && _attackDelayTimer.IsCompleted,
                    new Type[]{typeof(ChaseState), typeof(PatrolState)}),
                new FiniteTransition<AttackState, ChaseState>(() => _targeter.IsTargeting),
                new FiniteTransition<AttackState, PatrolState>(() => !_targeter.IsTargeting),
                new FiniteTransition<PatrolState, ChaseState>(() => _targeter.IsTargeting && (!_attackCooldownTimer.IsCompleted || !_attackDelayTimer.IsCompleted)),
                new FiniteTransition<ChaseState, PatrolState>(() => !_targeter.IsTargeting)
            };

            _finiteStateMachine = new FiniteStateMachine();
            _finiteStateMachine.AddStates(patrolState, chaseState, attackState);
            _finiteStateMachine.AddTransitions(transitions);
            _finiteStateMachine.Init(typeof(PatrolState));

            _creature.OnDispose.Take(1).Subscribe(_ => Dispose());
        }

        public void UpdateLogic()
        {
            _finiteStateMachine.CurrentState.UpdateLogic();

            _attackDelayTimer.Tick();
            _attackCooldownTimer.Tick();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
