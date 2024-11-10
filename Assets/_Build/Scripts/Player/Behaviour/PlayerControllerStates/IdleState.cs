using Assets._Build.Scripts.Architecture.FSM;

namespace Assets._Build.Scripts.Player.Behaviour.PlayerControllerStates
{
    public class IdleState: FiniteState
    {
        public override void UpdateLogic()
        {
            HandleTransitions();
        }
    }
}
