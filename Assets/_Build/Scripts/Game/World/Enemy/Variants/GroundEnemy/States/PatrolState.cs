using System;
using UnityEngine;
using R3;

using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Enemy.EnemyStates;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.StateParameters;

namespace LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States
{
    public class PatrolState : EnemyState
    {
        private readonly Transform[] _patrolPoints;
        private PatrolParameters _patrolParameters;
        private float _finishTime;
        private int _currentPatrolPointIndex;
        private IDisposable _disposable;

        public PatrolState(Agent agent, Transform[] patrolPoints) : base(agent)
        {
            _patrolPoints = patrolPoints;
        }

        public void Init(PatrolParameters parameters)
        {
            _patrolParameters = parameters;
        }

        public override void Enter()
        {
            _disposable = _agent.IsStopped.Subscribe(e =>
            {
                if (e)
                {
                    SetFinishPatrolTime();
                }
            });
            _agent.SetDestination(_patrolPoints[GetNextPatrolPointIndex()].position);
        }

        public override void Exit()
        {
            _disposable?.Dispose();
        }

        public override void UpdateLogic()
        {
            Patrol();
            HandleTransitions();
        }

        private void SetFinishPatrolTime()
        {
            _finishTime = Time.time;
        }

        private int GetNextPatrolPointIndex()
        {
            var lastIndex = _patrolPoints.Length - 1;

            if (_currentPatrolPointIndex == lastIndex)
            {
                _currentPatrolPointIndex = 0;
                return lastIndex;
            }
            else
            {
                return _currentPatrolPointIndex++;
            }
        }

        private void Patrol()
        {
            if ((_finishTime + _patrolParameters.WaitTime < Time.time) && _agent.IsStopped.CurrentValue)
            {
                var nextPatrolPoint = GetNextPatrolPointIndex();
                _agent.SetDestination(_patrolPoints[nextPatrolPoint].position);
            }
        }
    }
}
