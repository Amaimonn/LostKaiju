using UnityEngine;

using Assets._Build.Scripts.Agents;
using Assets._Build.Scripts.Architecture.FSM;
using Assets._Build.Scripts.Enemy.StateParameters;
using Assets._Build.Scripts.Enemy.EnemyStates;

namespace Assets._Build.Scripts.Enemy
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
            //_finiteStateMachine.SetTransitionsWithStates(null, new FiniteState[1]{patrolState});
            _finiteStateMachine.AddState(patrolState);
            _finiteStateMachine.Init();
        }

        private void Update()
        {
            _finiteStateMachine.CurrentState.UpdateLogic();
        }
    }
}
