using System;
using UnityEngine;
using R3;

namespace LostKaiju.Boilerplates.UI.MVVM
{
    public abstract class View : MonoBehaviour, IDisposable
    {
        public Observable<Unit> OnDisposed => _onDisposed;
        public Subject<Unit> _onDisposed = new();
        public abstract void Attach(IRootUI rootUI);
        public abstract void Detach(IRootUI rootUI);
        
        public virtual void Dispose()
        {
            _onDisposed.OnNext(Unit.Default);
        }
    }

    public abstract class View<T> : View where T : IViewModel
    {
        protected T ViewModel { get; private set; }

        public void Bind(T viewModel)
        {
            ViewModel = viewModel;
            OnBind(viewModel);
        }

        protected abstract void OnBind(T viewModel);
    }
}
