using LostKaiju.Gameplay.Player.Data.Model;
using LostKaiju.Models.UI.MVVM;

namespace LostKaiju.Gameplay.UI.MVVM.Gameplay
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