using System;
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
        public Observable<SettingsViewModel> OnOpened => _onOpened;
        
        private readonly Subject<SettingsViewModel> _onOpened = new();
        private IRootUIBinder _rootUIBinder;
        private IGameStateProvider _gameStateProvider;
        private SettingsModel _settingsModel;
        // private ApplyPopUpBinder _applyPopUpBinder
        private SettingsViewModel _currentSettingsViewModel;
        private bool _isClosingEnabled = true;

        public SettingsBinder(IRootUIBinder rootUIBinder, IGameStateProvider gameStateProvider, 
            SettingsModel settingsModel)
        {
            _rootUIBinder = rootUIBinder;
            _gameStateProvider = gameStateProvider;
            _settingsModel = settingsModel;
        }

        public IDisposable BindClosingSignal(Observable<Unit> onClose)
        {
            return onClose.Subscribe(_ => 
            {
                if (!_isClosingEnabled)
                    return;

                if (_currentSettingsViewModel != null)
                {
                    if (_currentSettingsViewModel.IsApplyPopUpOpened) // if _applyPopUpBinder.CurrentViewModel != null
                    {
                        // _isClosingEnabled = false;
                        // _applyPopUpBinder.CurrentViewModel.OnClosingCompleted.Take(1).Subscribe(_ => _isClosingEnabled = true);
                        // _applyPopUpBinder.CurrentViewModel.StartClosing();
                    }
                    else
                    {
                        _isClosingEnabled = false;
                        _currentSettingsViewModel.OnClosingCompleted.Take(1).Subscribe(_ => _isClosingEnabled = true);
                        _currentSettingsViewModel.StartClosing();
                        Debug.Log("SettingsBinder catched esc");
                    }
                }
            });
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
            _onOpened.OnNext(_currentSettingsViewModel);
            
            return _currentSettingsViewModel;
        }
    }
}