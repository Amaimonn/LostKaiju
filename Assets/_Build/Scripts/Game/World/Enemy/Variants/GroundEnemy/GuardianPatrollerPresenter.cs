using System;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.FSM;
using LostKaiju.Boilerplates.FSM.FiniteTransitions;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.StateParameters;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Agents.Sensors;
using LostKaiju.Utils;

namespace LostKaiju.Game.World.Enemy
{
    public class GuardianPatrollerPresenter : MonoBehaviour
    {
        [SerializeField] private CreatureBinder _creatureBinder;
        [SerializeField] private GroundAgent _groundAgent;
        [SerializeField] private OccludablePlayerSensor _playerSensor;
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _attackDistance = 5;

        private Timer _attackCooldownTimer;

        private FiniteStateMachine _finiteStateMachine;

        private void Awake()
        {
            _creatureBinder.Init();

            var targeter = _creatureBinder.Features.Resolve<ITargeter>();
            var attacker = _creatureBinder.Features.Resolve<IAttacker>();

            _playerSensor.Detected.Skip(1).Subscribe(x =>
            {
                if (x != null)
                {
                    Debug.Log(x.name);
                    targeter.SetTarget(x.transform);
                }
                else
                {
                    targeter.SetTarget(null);
                }
            });

            var patrolState = new PatrolState(_groundAgent, _patrolPoints);
            patrolState.Init(new PatrolParameters());

            var chaseState = new ChaseState(_groundAgent, targeter);
            chaseState.Init(_attackDistance);

            var attackState = new AttackState(_groundAgent, attacker);
            _attackCooldownTimer = new Timer(_attackCooldown, true);
            attackState.OnEnter.Subscribe(x =>
            {
                _attackCooldownTimer.Refresh();
            });

            var transitions = new IFiniteTransition[]
            {
                new SameForMultipleTransition<AttackState>(() => targeter.IsTargeting == true && _attackCooldownTimer.IsCompleted,
                    new Type[]{typeof(ChaseState), typeof(PatrolState)}),
                new FiniteTransition<AttackState, ChaseState>(() => targeter.IsTargeting == true),
                new FiniteTransition<AttackState, PatrolState>(() => targeter.IsTargeting == false),
                new FiniteTransition<PatrolState, ChaseState>(() => targeter.IsTargeting && !_attackCooldownTimer.IsCompleted),
                new FiniteTransition<ChaseState, PatrolState>(() => targeter.IsTargeting == false)
            };

            _finiteStateMachine = new FiniteStateMachine();
            _finiteStateMachine.AddStates(patrolState, chaseState, attackState);
            _finiteStateMachine.AddTransitions(transitions);
            _finiteStateMachine.Init(typeof(PatrolState));
        }

        private void Update()
        {
            _finiteStateMachine.CurrentState.UpdateLogic();

            _attackCooldownTimer.Tick();
        }
    }
}
