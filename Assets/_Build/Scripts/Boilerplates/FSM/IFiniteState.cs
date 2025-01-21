using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

using LostKaiju.Boilerplates.FSM.FiniteTransitions;
using UnityEngine;

namespace LostKaiju.Boilerplates.FSM
{
    public interface IFiniteState: IDisposable
    {
        public Observable<Type> OnTransition { get; } // emit ToState type
        public Observable<Unit> OnEnter { get; }
        public Observable<Unit> OnExit { get; }

        public void Enter();

        public void Exit();

        public virtual void UpdateLogic() {}

        public virtual void FixedUpdateLogic() {}

        public void SetTransitions(IObservableCollection<IFiniteTransition> observableTransitions);

        public void AddTransitions(IObservableCollection<IFiniteTransition> observableTransitions);
        
        public void HandleTransitions();
    }
}