using System;

namespace Assets._Build.Scripts.Architecture.FSM.FiniteTransitions
{
    public class FiniteTransition<TFrom, TTo> : IFiniteTransition
        where TFrom : FiniteState
        where TTo : FiniteState
    {
        public Type FromStateType => typeof(TFrom);
        public Type ToStateType => typeof(TTo);
        
        public Func<bool> Condition {get;}

        public FiniteTransition(Func<bool> condition)
        {
            Condition = condition;
        }
    }
}
