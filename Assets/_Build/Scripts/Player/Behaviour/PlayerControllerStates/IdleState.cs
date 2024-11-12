using LostKaiju.Architecture.FSM;

namespace LostKaiju.Player.Behaviour.PlayerControllerStates
{
    public class IdleState: FiniteState
    {
        public override void UpdateLogic()
        {
            HandleTransitions();
        }
    }
}
