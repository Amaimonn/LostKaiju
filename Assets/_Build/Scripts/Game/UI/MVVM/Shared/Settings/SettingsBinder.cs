using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsBinder
    {
        private IRootUIBinder _rootUIBinder;
        private IGameStateProvider _gameStateProvider;
        private SettingsModel _settingsModel;
        private SettingsViewModel _currentSettingsViewModel;

        public SettingsBinder(IRootUIBinder rootUIBinder, IGameStateProvider gameStateProvider, 
            SettingsModel settingsModel)
        {
            _rootUIBinder = rootUIBinder;
            _gameStateProvider = gameStateProvider;
            _settingsModel = settingsModel;
        }

        public SettingsViewModel ShowSettings()
        {
            if (_currentSettingsViewModel != null) // if already exists
                return null;

            var settingsDataSO = Resources.Load<FullSettingsDataSO>(Paths.FULL_SETTINGS_DATA_SO);
            var settingsViewPrefab = Resources.Load<SettingsView>(Paths.SETTINGS_VIEW);
            var settingsView = UnityEngine.Object.Instantiate(settingsViewPrefab);
            
            _currentSettingsViewModel = new SettingsViewModel(_settingsModel, settingsDataSO, _gameStateProvider);
            
            _currentSettingsViewModel.OnClosingCompleted.Subscribe(_ => {
                _rootUIBinder.ClearView(settingsView);
            });
            settingsView.OnDisposed.Take(1).Subscribe(_ => {
                _currentSettingsViewModel?.Dispose();
                _currentSettingsViewModel = null;
            });

            settingsView.Bind(_currentSettingsViewModel);
            _rootUIBinder.AddView(settingsView);
            _currentSettingsViewModel.Open();
            
            return _currentSettingsViewModel;
        }
    }
}