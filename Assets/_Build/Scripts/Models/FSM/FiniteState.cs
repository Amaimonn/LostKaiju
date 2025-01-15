using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

using LostKaiju.Models.FSM.FiniteTransitions;
using UnityEngine;

namespace LostKaiju.Models.FSM
{
    public abstract class FiniteState: IFiniteState
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
            _transitions.Clear();

            _disposables.Dispose();
            _disposables = new();
            
            AddTransitions(observableTransitions);
        }
        
        public virtual void AddTransitions(IObservableCollection<IFiniteTransition> observableTransitions)
        {
            if (observableTransitions == null)
            {
                Debug.LogWarning("Observable transitions are null");
                return;
            }

            var selfType = GetType();
            _transitions = observableTransitions.Where(x => x.CheckFromStateType(selfType)).ToList();

            observableTransitions
                .ObserveAdd()
                .Select(e => e.Value)
                .Where(x => x.CheckFromStateType(selfType))
                .Subscribe(x => _transitions.Add(x))
                .AddTo(_disposables);

            observableTransitions
                .ObserveRemove()
                .Select(e => e.Value)
                .Where(x => x.CheckFromStateType(selfType))
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
