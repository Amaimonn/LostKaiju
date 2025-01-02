using LostKaiju.Models.FSM;

namespace LostKaiju.Gameplay.Player.Behaviour.PlayerControllerStates
{
    public class IdleState: FiniteState
    {
        public override void UpdateLogic()
        {
            HandleTransitions();
        }
    }
}
