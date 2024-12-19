using LostKaiju.Player.Data.Model;

namespace LostKaiju.UI.MVVM.Gameplay
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