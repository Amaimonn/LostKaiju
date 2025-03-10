using UnityEngine;
using R3;

using LostKaiju.Game.World.Agents;
using LostKaiju.Boilerplates.FSM;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.StateParameters;
using LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Agents.Sensors;

namespace LostKaiju.Game.World.Enemy
{
    public class GuardianPatrollerPresenter : MonoBehaviour
    {
        [SerializeField] private CreatureBinder _creatureBinder;
        [SerializeField] private GroundAgent _groundAgent;
        [SerializeField] private OccludablePlayerSensor _playerSensor;
        [SerializeField] private Transform[] _patrolPoints;

        private FiniteStateMachine _finiteStateMachine;

        private void Awake()
        {
            _creatureBinder.Init();

            var targeter = _creatureBinder.Features.Resolve<ITargeter>(); // test
            _playerSensor.Detected.Subscribe( x => {
                if (x != null)
                    targeter.SetTarget(x.transform);
                else
                    targeter.SetTarget(null);
            });

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
