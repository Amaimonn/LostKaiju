using R3;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public abstract class ScreenCanvasView<T> : CanvasView<T> where T : ScreenViewModel
    {
        protected CompositeDisposable _disposables = new();

        protected override void OnBind(T viewModel)
        {
            ViewModel.OnOpenStateChanged.Skip(1).Subscribe(OnOpenStateChanged).AddTo(_disposables);
        }

        protected virtual void OnOpenStateChanged(bool isOpened)
        {
            if (isOpened)
                OnOpening();
            else
                OnClosing();
        }

        protected virtual void OnOpening()
        {

        }

        protected virtual void OnClosing()
        {
            ViewModel.CompleteClosing();
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}