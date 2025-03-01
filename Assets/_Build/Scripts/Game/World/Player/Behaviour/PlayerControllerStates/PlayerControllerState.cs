using LostKaiju.Boilerplates.FSM;
// using LostKaiju.Services.Inputs;

namespace LostKaiju.Game.World.Player.Behaviour.PlayerControllerStates
{
    public abstract class PlayerControllerState : FiniteState
    {
        // protected IInputProvider _inputProvider;

        // public class Factory
        // {
        //     private readonly IInputProvider _inputProviderInjection;

        //     public Factory(IInputProvider inputProvider)
        //     {
        //         _inputProviderInjection = inputProvider;
        //     }

        //     public T Create<T>() where T : PlayerControllerState, new()
        //     {
        //         var state = new T
        //         {
        //             _inputProvider = _inputProviderInjection
        //         };
                
        //         return state;
        //     }
        // }
    }
}
