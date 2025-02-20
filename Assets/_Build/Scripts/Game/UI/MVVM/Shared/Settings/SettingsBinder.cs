using System;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsBinder : IDisposable
    {
        private readonly IRootUIBinder _rootUIBinder;
        private readonly SettingsModel _settingsModel;
        private SettingsViewModel _currentSettingsViewModel;
        private const string SETTINGS_VIEW_PATH = "UI/Shared/SettingsView";

        public SettingsBinder(IRootUIBinder rootUIBinder, SettingsModel settingsModel)
        {
            _rootUIBinder = rootUIBinder;
            _settingsModel = settingsModel;
        }

        public SettingsViewModel ShowSettings()
        {
            if (_currentSettingsViewModel != null) // already exists
                return null;

            var settingsViewPrefab = Resources.Load<SettingsView>(SETTINGS_VIEW_PATH);
            var settingsView = UnityEngine.Object.Instantiate(settingsViewPrefab);
            
            _currentSettingsViewModel = new SettingsViewModel(_settingsModel);
            
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