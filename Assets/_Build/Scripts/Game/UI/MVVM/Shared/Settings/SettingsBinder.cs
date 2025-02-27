using System;
using UnityEngine;
using VContainer;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.Constants;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.GameState;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsBinder : IDisposable
    {
        private IRootUIBinder _rootUIBinder;
        private IGameStateProvider _gameStateProvider;
        private SettingsModel _settingsModel;
        private SettingsViewModel _currentSettingsViewModel;
        private readonly string _settingsDataPath;

        [Inject]
        public void Construct(IRootUIBinder rootUIBinder, IGameStateProvider gameStateProvider, 
            SettingsModel settingsModel)
        {
            _rootUIBinder = rootUIBinder;
            _gameStateProvider = gameStateProvider;
            _settingsModel = settingsModel;
        }

        public SettingsBinder(string settingsDataPath)
        {
            _settingsDataPath = settingsDataPath;
        }

        public SettingsViewModel ShowSettings()
        {
            if (_currentSettingsViewModel != null) // if already exists
                return null;
            var settingsDataSO = Resources.Load<FullSettingsDataSO>(_settingsDataPath);
            var settingsViewPrefab = Resources.Load<SettingsView>(Paths.SETTINGS_VIEW_PATH);
            var settingsView = UnityEngine.Object.Instantiate(settingsViewPrefab);
            
            _currentSettingsViewModel = new SettingsViewModel(_settingsModel, settingsDataSO, _gameStateProvider);
            
            _currentSettingsViewModel.OnClosingCompleted.Subscribe(_ => {
                _rootUIBinder.ClearView(settingsView);
                _currentSettingsViewModel?.Dispose();
                _currentSettingsViewModel = null;
            });
            settingsView.Bind(_currentSettingsViewModel);
            _rootUIBinder.AddView(settingsView);
            _currentSettingsViewModel.Open();
            
            return _currentSettingsViewModel;
        }

        public void Dispose() // with VContainer on Scope destroy
        {
            if (_currentSettingsViewModel != null)
            {
                _currentSettingsViewModel.Dispose();
                _currentSettingsViewModel = null;
            }
        }
    }
}