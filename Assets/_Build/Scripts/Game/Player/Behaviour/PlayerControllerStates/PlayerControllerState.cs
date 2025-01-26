using LostKaiju.Boilerplates.FSM;
using LostKaiju.Services.Inputs;
using LostKaiju.Boilerplates.Locator;

namespace LostKaiju.Game.Player.Behaviour.PlayerControllerStates
{
    public abstract class PlayerControllerState : FiniteState
    {
        protected readonly IInputProvider _inputProvider;
        
        public PlayerControllerState() : base()
        {
            _inputProvider = ServiceLocator.Instance.Get<IInputProvider>();
        }
    }
}
