using R3;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class ExitPopUpViewModel : ScreenViewModel
    {
        public Observable<Unit> OnExitConfirmed => _onExitConfirmed;
        private Subject<Unit> _onExitConfirmed = new();

        public void ConfirmExit()
        {
            _onExitConfirmed.OnNext(Unit.Default);
            // StartClosing();
        }
    }
}