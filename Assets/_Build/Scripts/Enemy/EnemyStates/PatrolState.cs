using UnityEngine;
using R3;
using System;

using Assets._Build.Scripts.Architecture.FSM;
using Assets._Build.Scripts.Agents;
using Assets._Build.Scripts.Enemy.StateParameters;

namespace Assets._Build.Scripts.Enemy.EnemyStates
{
    public class PatrolState: FiniteState
    {
        private Agent _agent;
        private Transform[] _patrolPoints;
        private PatrolParameters _patrolParameters;
        private float _finishTime;
        private int _currentPatrolPointIndex;
        private IDisposable _disposable;
        
        public PatrolState(Agent agent, Transform[] patrolPoints)
        {
            _agent = agent;
            _patrolPoints = patrolPoints;
        }

        public void Init(PatrolParameters parameters)
        {
            _patrolParameters = parameters;
        }

        public override void Enter()
        {
            _disposable = _agent.IsStopped.Subscribe(e => {
                if(e)
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
        }

        private void SetFinishPatrolTime()
        {
            _finishTime = Time.time;
        }

        private int GetNextPatrolPointIndex()
        {
            var lastIndex = _patrolPoints.Length-1;

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
            if((_finishTime + _patrolParameters.WaitTime < Time.time) && _agent.IsStopped.CurrentValue)   
            {
                var nextPatrolPoint = GetNextPatrolPointIndex();
                _agent.SetDestination(_patrolPoints[nextPatrolPoint].position);
            }
        }
    }
}
