using LostKaiju.Game.UI.MVVM.Shared.Settings;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class GameplayViewModel : ScreenViewModel
    {
        private readonly OptionsBinder _optionsBinder;

        public GameplayViewModel(OptionsBinder optionsBinder)
        {
            _optionsBinder = optionsBinder;
        }

        public void OpenOptions()
        {
            _optionsBinder.ShowOptions();
        }
    }
}