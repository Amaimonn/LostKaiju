using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.Constants;
using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.Providers.InputState;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class OptionsBinder
    {
        private readonly IRootUIBinder _rootUIBinder;
        private readonly InputStateProvider _inputStateProvider;
        private readonly SettingsBinder _settingsBinder;
        private readonly ExitPopUpBinder _exitPopUpBinder;
        private OptionsViewModel _currentOptionsViewModel;

        public OptionsBinder(IRootUIBinder rootUIBinder, InputStateProvider inputStateProvider, 
            SettingsBinder settingsBinder, ExitPopUpBinder exitPopUpBinder)
        {
            _rootUIBinder = rootUIBinder;
            _inputStateProvider = inputStateProvider;
            _settingsBinder = settingsBinder;
            _exitPopUpBinder = exitPopUpBinder;
        }

        public OptionsViewModel ShowOptions()
        {
            if (_currentOptionsViewModel != null)
                return null;

            // TODO: disable/enable input through mediator/interface
            var optionsViewPrefab = Resources.Load<OptionsView>(Paths.OPTIONS_VIEW_PATH);
            var optionsView = UnityEngine.Object.Instantiate(optionsViewPrefab);
            _inputStateProvider.AddBlocker(optionsView);
            
            _currentOptionsViewModel = new OptionsViewModel(_settingsBinder, _exitPopUpBinder);
            
            _currentOptionsViewModel.OnClosingCompleted.Subscribe(_ => {
                _rootUIBinder.ClearView(optionsView);
            });
            optionsView.OnDisposed.Take(1).Subscribe(_ => {
                // _currentOptionsViewModel?.Dispose();
                _inputStateProvider.RemoveBlocker(optionsView);
                _currentOptionsViewModel = null;
            });
            
            optionsView.Bind(_currentOptionsViewModel);
            _rootUIBinder.AddView(optionsView);
            _currentOptionsViewModel.Open();
            
            return _currentOptionsViewModel;
        }
    }
}