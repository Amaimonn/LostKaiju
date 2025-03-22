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
            _settingsBinder.TryBindAndOpen(out _);
        }
    }
}