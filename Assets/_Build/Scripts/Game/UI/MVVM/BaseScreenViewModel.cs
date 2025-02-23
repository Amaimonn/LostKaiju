using R3;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM
{
    public abstract class BaseScreenViewModel : IViewModel
    {
        public Observable<bool> OnOpenStateChanged => _isOpened;
        public Observable<Unit> OnClosingCompleted => _closingCompletedSignal;

        protected readonly ReactiveProperty<bool> _isOpened = new(false);
        protected readonly Subject<Unit> _closingCompletedSignal = new();
        
        public virtual void Open()
        {
            _isOpened.Value = true;
        }

        public virtual void StartClosing()
        {
            _isOpened.Value = false;
        }

        /// <summary>
        /// Complete closing when animation is finished. Used by View.
        /// </summary>
        public virtual void CompleteClosing()
        {
            _closingCompletedSignal.OnNext(Unit.Default);
        }
    }
}