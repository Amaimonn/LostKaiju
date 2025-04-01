using LostKaiju.Game.UI.MVVM.Shared.Settings;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class GameplayViewModel : ScreenViewModel
    {
        private readonly OptionsBinder _optionsBinder;
        private bool _isMissionCompletionPopUpOpened;
        public GameplayViewModel(OptionsBinder optionsBinder)
        {
            _optionsBinder = optionsBinder;
        }

        public void OpenOptions()
        {
            if (_isMissionCompletionPopUpOpened)
                return;
            _optionsBinder.ShowOptions();
        }

        public void OpenMissionCompletionPopUp() // mb some IStatistics in args 
        {
            _optionsBinder.CloseAll();
            // TODO: show pop up
            _isMissionCompletionPopUpOpened = true;
        }
    }
}