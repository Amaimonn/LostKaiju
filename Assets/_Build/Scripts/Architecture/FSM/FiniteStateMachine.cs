using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

using Assets._Build.Scripts.Architecture.FSM.FiniteTransitions;

namespace Assets._Build.Scripts.Architecture.FSM
{
    public abstract class FiniteStateMachine : IDisposable
    {
        public FiniteState CurrentState { get; protected set; }

        protected Type _currentStateType;
        protected Dictionary<Type, FiniteState> _states = new();
        protected ObservableList<IFiniteTransition> _transitions = new();
        protected CompositeDisposable _disposables = new();

        public FiniteStateMachine(Type startStateType)
        {
            _currentStateType = startStateType;
        }

        public void Init()
        {
            if (_currentStateType != null)
            {
                if (_states.TryGetValue(_currentStateType, out var state))
                {
                    CurrentState = state;
                    CurrentState.Enter();
                }
            }
        }

        public abstract void ChangeState(Type stateType);

        public virtual void SetTransitionsWithStates(IObservableCollection<IFiniteTransition> observableTransitions, 
            IEnumerable<FiniteState> states)
        {
            _states = new();
            _transitions?.Clear();
            if (observableTransitions != null)
            {
                _transitions = new ObservableList<IFiniteTransition>(observableTransitions);
            }

            _disposables?.Dispose();
            _disposables = new CompositeDisposable();
            

            foreach (var state in states)
            {
                state.SetTransitions(_transitions);
                state.OnTransition.Subscribe(ChangeState).AddTo(_disposables);
                _states[state.GetType()] = state;
            }
        }

        public virtual void Reset()
        {
            foreach (var state in _states.Values)
            {
                state.Dispose();
            }
            _states = new();
            _transitions?.Clear();
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();
        }

        public virtual void AddTransitions(IEnumerable<IFiniteTransition> transitions)
        {
            _transitions.AddRange(transitions);
        }

        public virtual void AddState<T>(T state) where T : FiniteState
        {
            RemoveState<T>();
            state.SetTransitions(_transitions);
            state.OnTransition.Subscribe(ChangeState).AddTo(_disposables);
            _states[typeof(T)] = state;
        }

        public virtual void RemoveState<T>()
        {
            var removedType = typeof(T);
            if (_states.TryGetValue(removedType, out var state))
            {
                state.Dispose();
                if (_transitions!= null)
                {
                    var transitionsToRemove = _transitions.Where(x => x.ToStateType == removedType);
                    foreach (var transition in transitionsToRemove)
                    {
                        _transitions.Remove(transition);
                    }
                }
            }
        }
        
        public virtual void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
