using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ObservableCollections;
using R3;

using LostKaiju.Boilerplates.FSM.FiniteTransitions;

namespace LostKaiju.Boilerplates.FSM
{
    public abstract class BaseFiniteStateMachine : IFiniteStateMachine
    {
        public IFiniteState CurrentState { get; protected set; }

        protected Type _currentStateType;
        protected Dictionary<Type, IFiniteState> _states = new();
        protected ObservableList<IFiniteTransition> _transitions = new();
        protected CompositeDisposable _disposables = new();

        public virtual void Init(Type startStateType)
        {
            if (startStateType != null)
            {
                _currentStateType = startStateType;
                if (_states.TryGetValue(_currentStateType, out var state))
                {
                    CurrentState = state;
                    CurrentState.Enter();
                }
                else
                {
                    Debug.LogWarning($"FSM warning: Can`t find '{_currentStateType.Name}' state for initialization");
                }
            }
            else
            {
                Debug.LogWarning("FSM warning: Start state type is null");
            }
        }

        public abstract void ChangeState(Type stateType);

        public virtual void SetTransitionsWithStates(IObservableCollection<IFiniteTransition> observableTransitions, 
            IEnumerable<IFiniteState> states)
        {
            _states.Clear();
            _transitions.Clear();
            if (observableTransitions != null)
            {
                _transitions = new ObservableList<IFiniteTransition>(observableTransitions);
            }

            _disposables.Dispose();
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
            
            _states.Clear();
            _transitions.Clear();
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        public virtual void AddTransitions(IEnumerable<IFiniteTransition> transitions)
        {
            _transitions.AddRange(transitions);
        }

        /// <summary>
        /// Adds the state in finite states machine with specified type as a key.
        /// Generic option can be used to add state with it`s parent type.
        /// </summary>
        /// <typeparam name="T">The key type for the state registration</typeparam>
        /// <param name="state"></param>
        public virtual void AddState<T>(T state) where T : IFiniteState
        {
            AddStateWithType(state, typeof(T));
        }

        /// <summary>
        /// Add several states to the finite state machine at once.
        /// In case of adding any state with it`s parent type use AddState to add this state instead.
        /// </summary>
        /// <param name="states"></param>
        public virtual void AddStates(params IFiniteState[] states)
        {
            foreach (var state in states)
            {
                AddStateWithType(state, state.GetType());
            }
        }

        public virtual void RemoveState<T>()
        {
            var removeType = typeof(T);
            RemoveStateByType(removeType);
        }

        protected virtual void AddStateWithType(IFiniteState state, Type type)
        {
            RemoveStateByType(type);
            state.SetTransitions(_transitions);
            state.OnTransition.Subscribe(ChangeState).AddTo(_disposables);
            _states[type] = state;
        }

        protected virtual void RemoveStateByType(Type type)
        {
            if (_states.TryGetValue(type, out var state))
            {
                state.Dispose();
                if (_transitions!= null)
                {
                    var transitionsToRemove = _transitions.Where(x => x.ToStateType == type);
                    foreach (var transition in transitionsToRemove)
                    {
                        _transitions.Remove(transition);
                    }
                }
            }
        }

#region IDisposable
        public virtual void Dispose()
        {
            _disposables.Dispose();
        }
#endregion
    }
}
