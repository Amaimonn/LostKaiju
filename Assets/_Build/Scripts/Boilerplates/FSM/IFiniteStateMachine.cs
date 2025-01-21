using System;
using System.Collections.Generic;
using ObservableCollections;

using LostKaiju.Boilerplates.FSM.FiniteTransitions;

namespace LostKaiju.Boilerplates.FSM
{
    public interface IFiniteStateMachine : IDisposable
    {
        public IFiniteState CurrentState { get; }

        public void Init(Type startStateType);

        public void ChangeState(Type stateType);

        public void SetTransitionsWithStates(IObservableCollection<IFiniteTransition> observableTransitions,
            IEnumerable<IFiniteState> states);

        public void Reset();

        public void AddTransitions(IEnumerable<IFiniteTransition> transitions);

        /// <summary>
        /// Adds the state in finite states machine with specified type as a key.
        /// Generic option can be used to add state with it`s parent type.
        /// </summary>
        /// <typeparam name="T">The key type for the state registration</typeparam>
        /// <param name="state"></param>
        public void AddState<T>(T state) where T : IFiniteState;

        /// <summary>
        /// Add several states to the finite state machine at once.
        /// When adding a state with it`s parent type (as key) use AddState to add this state instead of this method.
        /// </summary>
        /// <param name="states"></param>
        public void AddStates(params IFiniteState[] states);

        public void RemoveState<T>();
    }
}