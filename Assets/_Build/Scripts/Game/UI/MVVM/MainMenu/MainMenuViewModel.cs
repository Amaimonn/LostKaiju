using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Shared.Settings;

namespace LostKaiju.Game.UI.MVVM.MainMenu
{
    public class MainMenuViewModel : IViewModel
    {
        private readonly Subject<Unit> _exitSubject;
        private readonly SettingsBinder _settingsBinder;
        private bool _isSettingsOpened = false;

        public MainMenuViewModel(Subject<Unit> exitSubject, SettingsBinder settingsBinder)
        {
            _exitSubject = exitSubject;
            _settingsBinder = settingsBinder;
        }

        public void StartGameplay()
        {
            Debug.Log("Start Gameplay signal in vm");
            _exitSubject.OnNext(Unit.Default);
        }

        public void OpenSettings()
        {
            if (_isSettingsOpened)
                return;

            var settingsViewModel = _settingsBinder.ShowSettings();
            settingsViewModel?.OnClosingCompleted.Subscribe(_ =>  {
                _isSettingsOpened = false;
            });
            
            _isSettingsOpened = true;
        }
    }
}