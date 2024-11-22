using UnityEngine;

using LostKaiju.Agents;
using LostKaiju.Architecture.FSM;
using LostKaiju.Enemy.StateParameters;
using LostKaiju.Enemy.EnemyStates;

namespace LostKaiju.Enemy
{
    public class GroundEnemy : MonoBehaviour
    {
        [SerializeField] private GroundAgent _groundAgent;
        [SerializeField] private Transform[] _patrolPoints;

        private FiniteStateMachine _finiteStateMachine;

        private void Awake()
        {
            var patrolState = new PatrolState(_groundAgent, _patrolPoints);
            patrolState.Init(new PatrolParameters());

            _finiteStateMachine = new BaseFiniteStateMachine(typeof(PatrolState));
            _finiteStateMachine.AddState(patrolState);
            _finiteStateMachine.Init();
        }

        private void Update()
        {
            _finiteStateMachine.CurrentState.UpdateLogic();
        }
    }
}
