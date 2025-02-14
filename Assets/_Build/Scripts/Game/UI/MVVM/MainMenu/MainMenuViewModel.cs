using System;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.MainMenu
{
    public class MainMenuViewModel : IViewModel
    {
        private readonly Subject<Unit> _exitSubject;
        private readonly IRootUIBinder _rootUIBinder;
        private readonly Func<SettingsModel> _settingsModelFactory;
        private bool _isSettingsOpened = false;
        private const string SETTINGS_VIEW_PATH = "UI/Shared/SettingsView";

        public MainMenuViewModel(Subject<Unit> exitSubject, Func<SettingsModel> settingsModelFactory, 
            IRootUIBinder rootUIBinder)
        {
            _exitSubject = exitSubject;
            _settingsModelFactory = settingsModelFactory;
            _rootUIBinder = rootUIBinder;
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

            var settingsViewPrefab = Resources.Load<SettingsView>(SETTINGS_VIEW_PATH);
            var settingsView = UnityEngine.Object.Instantiate(settingsViewPrefab);
            var settingsModel = _settingsModelFactory();
            var settingsViewModel = new SettingsViewModel(settingsModel);
            settingsViewModel.OnClosingCompleted.Subscribe(_ =>  {
                _rootUIBinder.ClearView(settingsView);
                _isSettingsOpened = false;
            });

            settingsView.Bind(settingsViewModel);
            _rootUIBinder.AddView(settingsView);
            settingsViewModel.Open();
            _isSettingsOpened = true;
        }
    }
}