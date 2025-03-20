using System;
using System.Collections.Generic;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.Providers.InputState;
using LostKaiju.Game.Constants;
using UnityEngine.EventSystems;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class OptionsBinder
    {
        public Observable<OptionsViewModel> OnOpened => _onOpened;
        
        private readonly Subject<OptionsViewModel> _onOpened = new();
        private readonly IRootUIBinder _rootUIBinder;
        private readonly InputStateProvider _inputStateProvider;
        private readonly SettingsBinder _settingsBinder;
        private readonly ExitPopUpBinder _exitPopUpBinder;
        private OptionsViewModel _currentOptionsViewModel;
        private readonly Stack<ScreenViewModel> _screensStack = new();
        private CompositeDisposable _disposables = new();
        private bool _nextClosingEnabled = true;

        public OptionsBinder(IRootUIBinder rootUIBinder, InputStateProvider inputStateProvider,
            SettingsBinder settingsBinder, ExitPopUpBinder exitPopUpBinder)
        {
            _rootUIBinder = rootUIBinder;
            _inputStateProvider = inputStateProvider;
            _settingsBinder = settingsBinder;
            _exitPopUpBinder = exitPopUpBinder;

            settingsBinder.OnOpened.Subscribe(RegisterInStack).AddTo(_disposables);
            // settingsBinder.OnApplyPopUpOpened.Subscribe(RegisterInStack).AddTo(_disposables);
            exitPopUpBinder.OnOpened.Subscribe(RegisterInStack).AddTo(_disposables);
        }

        public IDisposable BindEscapeSignal(Observable<Unit> onEscape)
        {
            return onEscape.Subscribe(_ => EscapeButtonHandler()).AddTo(_disposables);
        }

        private void EscapeButtonHandler()
        {
            if (!_nextClosingEnabled)
                return;

            if (_screensStack.TryPeek(out var viewModel))
            {
                _nextClosingEnabled = false;
                viewModel.OnClosingCompleted.Take(1).Subscribe(_ => _nextClosingEnabled = true).AddTo(_disposables);
                viewModel.StartClosing();
            }
            else
            {
                ShowOptions();
            }
        }

        public void CloseAll()
        {
            while (_screensStack.TryPeek(out var viewModel))
            {
                viewModel.CompleteClosing();
            }
        }

        private void RegisterInStack(ScreenViewModel viewModel)
        {
            _screensStack.Push(viewModel);
            viewModel.OnClosingCompleted.Take(1).Subscribe(_ => _screensStack.Pop()).AddTo(_disposables);
        }

        public OptionsViewModel ShowOptions()
        {
            if (_currentOptionsViewModel != null)
                return null;
                
            var optionsViewPrefab = Resources.Load<OptionsView>(Paths.OPTIONS_VIEW);
            var optionsView = UnityEngine.Object.Instantiate(optionsViewPrefab);
            _inputStateProvider.AddBlocker(optionsView);

            _currentOptionsViewModel = new OptionsViewModel(_settingsBinder, _exitPopUpBinder);

            _currentOptionsViewModel.OnClosingCompleted.Subscribe(_ =>
            {
                _screensStack.Pop();
                _rootUIBinder.ClearView(optionsView);
            });
            optionsView.OnDisposed.Take(1).Subscribe(_ =>
            {
                // _currentOptionsViewModel?.Dispose();
                _inputStateProvider.RemoveBlocker(optionsView);
                _currentOptionsViewModel = null;
            });

            optionsView.Bind(_currentOptionsViewModel);
            _rootUIBinder.AddView(optionsView);
            _currentOptionsViewModel.Open();
            _screensStack.Push(_currentOptionsViewModel);
            _onOpened.OnNext(_currentOptionsViewModel);

            return _currentOptionsViewModel;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _disposables = null;
        }
    }
}