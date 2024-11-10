using System;

namespace Assets._Build.Scripts.Architecture.FSM.FiniteTransitions
{
    public interface IFiniteTransition
    {
        public Type FromStateType {get;}
        public Type ToStateType {get;}
        
        public Func<bool> Condition {get;}
    }
}
