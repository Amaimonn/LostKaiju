using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Constants;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class ExitPopUpBinder
    {
        public Observable<ExitPopUpViewModel> OnOpened => _onOpened;
        
        private readonly Subject<ExitPopUpViewModel> _onOpened = new();
        private readonly IRootUIBinder _rootUIBinder;
        private readonly AudioPlayer _audioPlayer;
        private readonly Subject<Unit> _exitSignal;
        private ExitPopUpViewModel _currentExitPopUpViewModel;

        public ExitPopUpBinder(IRootUIBinder rootUIBinder, Subject<Unit> exitSignal, AudioPlayer audioPlayer)
        {
            _rootUIBinder = rootUIBinder;
            _exitSignal = exitSignal;
            _audioPlayer = audioPlayer;
        }

        public ExitPopUpViewModel ShowExitPopUp()
        {
            if (_currentExitPopUpViewModel != null)
                return null;

            _currentExitPopUpViewModel = new ExitPopUpViewModel();
            var exitPopUpPrefab = Resources.Load<ExitPopUpView>(Paths.EXIT_POPUP_VIEW);
            var exitPopUpView = Object.Instantiate<ExitPopUpView>(exitPopUpPrefab);
            exitPopUpView.Construct(_audioPlayer);

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
            _onOpened.OnNext(_currentExitPopUpViewModel);

            return _currentExitPopUpViewModel;
        }
    }
}