using System;

namespace LostKaiju.Architecture.FSM.FiniteTransitions
{
    public interface IFiniteTransition
    {
        public Type FromStateType {get;}
        public Type ToStateType {get;}
        
        public Func<bool> Condition {get;}
    }
}
