using System;

namespace LostKaiju.Models.FSM.FiniteTransitions
{
    public class FiniteTransition<TFrom, TTo> : IFiniteTransition
        where TFrom : FiniteState
        where TTo : FiniteState
    {
        public Type ToStateType => typeof(TTo);
        public Func<bool> Condition { get; }

        public bool CheckFromStateType(Type type) => type == typeof(TFrom);

        public FiniteTransition(Func<bool> condition)
        {
            Condition = condition;
        }
    }
}
