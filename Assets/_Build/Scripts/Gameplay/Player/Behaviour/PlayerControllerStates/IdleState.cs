using LostKaiju.Boilerplates.FSM;

namespace LostKaiju.Game.Player.Behaviour.PlayerControllerStates
{
    public class IdleState: FiniteState
    {
        public override void UpdateLogic()
        {
            HandleTransitions();
        }
    }
}
