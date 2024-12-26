using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

using LostKaiju.Models.FSM.FiniteTransitions;

namespace LostKaiju.Models.FSM
{
    public abstract class FiniteState: IDisposable
    {
        public Observable<Type> OnTransition => _onTransition;
        public Observable<Unit> OnEnter => _onEnter;
        public Observable<Unit> OnExit => _onExit;

        protected Subject<Type> _onTransition = new();
        protected Subject<Unit> _onEnter = new();
        protected Subject<Unit> _onExit = new();
        protected List<IFiniteTransition> _transitions = new();
        protected CompositeDisposable _disposables = new();

        public virtual void Enter() 
        {
            _onEnter.OnNext(Unit.Default);
        }

        public virtual void Exit() 
        {
            _onExit.OnNext(Unit.Default);
        }

        public virtual void UpdateLogic() {}

        public virtual void FixedUpdateLogic() {}

        public virtual void SetTransitions(IObservableCollection<IFiniteTransition> observableTransitions)
        {
            _transitions?.Clear();
            _transitions = new();

            _disposables?.Dispose();
            _disposables = new();
            
            AddTransitions(observableTransitions);
        }
        
        public virtual void AddTransitions(IObservableCollection<IFiniteTransition> observableTransitions)
        {
            if (observableTransitions == null)
                return;

            var selfType = GetType();
            _transitions = observableTransitions.Where(x => x.FromStateType == selfType).ToList();

            observableTransitions
                .ObserveAdd()
                .Select(e => e.Value)
                .Where(x => x.FromStateType == selfType)
                .Subscribe(x => _transitions.Add(x))
                .AddTo(_disposables);

            observableTransitions
                .ObserveRemove()
                .Select(e => e.Value)
                .Where(x => x.FromStateType == selfType)
                .Subscribe(x => _transitions.Remove(x))
                .AddTo(_disposables);
        }

        public virtual void HandleTransitions()
        {
            if (_transitions == null)
                return;

            foreach (var transition in _transitions)
            {
                if (transition.Condition())
                {
                    _onTransition.OnNext(transition.ToStateType);
                    return;
                }
            }
        }

        public virtual void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
