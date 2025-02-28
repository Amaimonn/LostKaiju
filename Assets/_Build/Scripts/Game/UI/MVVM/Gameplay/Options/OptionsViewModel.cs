using LostKaiju.Game.UI.MVVM.Shared.Settings;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class OptionsViewModel : ScreenViewModel
    {
        private readonly ExitPopUpBinder _exitPopUpBinder;
        private readonly SettingsBinder _settingsBinder;

        public OptionsViewModel(SettingsBinder settingsBinder, ExitPopUpBinder exitPopUpBinder)
        {
            _exitPopUpBinder = exitPopUpBinder;
            _settingsBinder = settingsBinder;
        }

        public void OpenExitPopUp()
        {
            _exitPopUpBinder.ShowExitPopUp();
        }

        public void OpenSettings()
        {
            _settingsBinder.ShowSettings();
        }
    }
}