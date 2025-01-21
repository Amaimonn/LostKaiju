using LostKaiju.Game.Player.Data.Models;
using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class GameplayViewModel : IViewModel
    {
        private readonly PlayerLiveParametersModel _playerLiveParameters;

        public GameplayViewModel(PlayerLiveParametersModel playerLiveParameters)
        {
            _playerLiveParameters = playerLiveParameters;
        }
    }
}