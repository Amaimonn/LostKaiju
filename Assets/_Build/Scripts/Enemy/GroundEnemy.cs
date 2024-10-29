using UnityEngine;

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