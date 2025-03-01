using UnityEngine;

using LostKaiju.Game.World.Agents;
using LostKaiju.Boilerplates.FSM;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.StateParameters;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States;

namespace LostKaiju.Game.World.Enemy
{
    public class GroundGuardianEnemy : MonoBehaviour
    {
        [SerializeField] private GroundAgent _groundAgent;
        [SerializeField] private Transform[] _patrolPoints;

        private BaseFiniteStateMachine _finiteStateMachine;

        private void Awake()
        {
            var patrolState = new PatrolState(_groundAgent, _patrolPoints);
            patrolState.Init(new PatrolParameters());

            _finiteStateMachine = new FiniteStateMachine();
            _finiteStateMachine.AddState(patrolState);
            _finiteStateMachine.Init(typeof(PatrolState));
        }

        private void Update()
        {
            _finiteStateMachine.CurrentState.UpdateLogic();
        }
    }
}
