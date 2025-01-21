using System;

namespace LostKaiju.Boilerplates.FSM.FiniteTransitions
{
    public interface IFiniteTransition
    {
        public Type ToStateType { get; }
        public Func<bool> Condition { get; }
        
        public bool CheckFromStateType(Type type);
    }
}
