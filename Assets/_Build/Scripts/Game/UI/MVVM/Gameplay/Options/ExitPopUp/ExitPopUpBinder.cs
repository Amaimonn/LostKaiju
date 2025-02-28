using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.Constants;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class ExitPopUpBinder
    {
        private readonly IRootUIBinder _rootUIBinder;
        private readonly Subject<Unit> _exitSignal;
        private ExitPopUpViewModel _currentExitPopUpViewModel;

        public ExitPopUpBinder(IRootUIBinder rootUIBinder, Subject<Unit> exitSignal)
        {
            _rootUIBinder = rootUIBinder;
            _exitSignal = exitSignal;
        }

        public ExitPopUpViewModel ShowExitPopUp()
        {
            if (_currentExitPopUpViewModel != null)
                return null;

            _currentExitPopUpViewModel = new ExitPopUpViewModel();
            var exitPopUpPrefab = Resources.Load<ExitPopUpView>(Paths.EXIT_POPUP_VIEW_PATH);
            var exitPopUpView = Object.Instantiate<ExitPopUpView>(exitPopUpPrefab);

            _currentExitPopUpViewModel.OnClosingCompleted.Subscribe(_ =>
            {
                _rootUIBinder.ClearView(exitPopUpView);
            });
            _currentExitPopUpViewModel.OnExitConfirmed.Take(1).Subscribe(_ =>
            {
                _exitSignal.OnNext(Unit.Default);
            });
            exitPopUpView.OnDisposed.Take(1).Subscribe(_ => {
                _currentExitPopUpViewModel = null;
                // exitPopUpViewModel?.Dispose();
            });

            exitPopUpView.Bind(_currentExitPopUpViewModel);
            _rootUIBinder.AddView(exitPopUpView);
            _currentExitPopUpViewModel.Open();

            return _currentExitPopUpViewModel;
        }
    }
}