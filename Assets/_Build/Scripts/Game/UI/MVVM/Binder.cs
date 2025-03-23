using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM
{
    public abstract class Binder<T> where T : IViewModel
    {
        public Observable<T> OnOpened => _onOpened;
        
        protected readonly Subject<T> _onOpened = new();
        protected readonly IRootUIBinder _rootUIBinder;
        protected T _currentViewModel;
        protected CompositeDisposable _disposables = new();

        public Binder(IRootUIBinder rootUIBinder)
        {
            _rootUIBinder = rootUIBinder;
        }

        public virtual TView LoadAndInstantiateView<TView>(string path) where TView : View<T>
        {
            var viewPrefab = Resources.Load<TView>(path);
            var view = UnityEngine.Object.Instantiate(viewPrefab);
            return view;
        }

        public abstract bool TryBindAndOpen(out T viewModel);

        public virtual void Dispose()
        {
            _disposables?.Dispose();
            _disposables = null;
        }
    }
}