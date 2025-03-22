using System;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.Constants;
using UnityEngine.AddressableAssets;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsBinder : Binder<SettingsViewModel>
    {
        private readonly IGameStateProvider _gameStateProvider;
        private readonly SettingsModel _settingsModel;
        // private ApplyPopUpBinder _applyPopUpBinder
        private bool _isClosingEnabled = true;

        public SettingsBinder(IRootUIBinder rootUIBinder, IGameStateProvider gameStateProvider, 
            SettingsModel settingsModel) : base(rootUIBinder)
        {
            _gameStateProvider = gameStateProvider;
            _settingsModel = settingsModel;
        }

        public IDisposable BindClosingSignal(Observable<Unit> onClose)
        {
            return onClose.Subscribe(_ => 
            {
                if (!_isClosingEnabled)
                    return;

                if (_currentViewModel != null)
                {
                    if (_currentViewModel.IsApplyPopUpOpened) // if _applyPopUpBinder.CurrentViewModel != null
                    {
                        // _isClosingEnabled = false;
                        // _applyPopUpBinder.CurrentViewModel.OnClosingCompleted.Take(1).Subscribe(_ => _isClosingEnabled = true);
                        // _applyPopUpBinder.CurrentViewModel.StartClosing();
                    }
                    else
                    {
                        _isClosingEnabled = false;
                        _currentViewModel.OnClosingCompleted.Take(1).Subscribe(_ => _isClosingEnabled = true);
                        _currentViewModel.StartClosing();
                        Debug.Log("SettingsBinder catched esc");
                    }
                }
            });
        }

        public override bool TryBindAndOpen(out SettingsViewModel viewModel)
        {
            if (_currentViewModel != null) // if already exists
            {
                viewModel = null;
                return false;
            }

            // var settingsDataSO = Resources.Load<FullSettingsDataSO>(Paths.FULL_SETTINGS_DATA_SO);
            var settingsDataSOHandle = Addressables.LoadAssetAsync<FullSettingsDataSO>(Paths.FULL_SETTINGS_DATA_SO);
            var settingsView = LoadAndInstantiateView<SettingsView>(Paths.SETTINGS_VIEW);
            _currentViewModel = new SettingsViewModel(_settingsModel, _gameStateProvider);
            settingsDataSOHandle.Completed += (handle) =>
            {
                _currentViewModel?.BindData(handle.Result);
            };
            _currentViewModel.OnClosingCompleted.Subscribe(_ => {
                _rootUIBinder.ClearView(settingsView);
            });
            settingsView.OnDisposed.Take(1).Subscribe(_ => {
                _currentViewModel?.Dispose();
                _currentViewModel = null;
                settingsDataSOHandle.Release();
            });

            settingsView.Bind(_currentViewModel);
            _rootUIBinder.AddView(settingsView);
            _currentViewModel.Open();
            _onOpened.OnNext(_currentViewModel);
            
            viewModel = _currentViewModel;
            return true;
        }
    }
}