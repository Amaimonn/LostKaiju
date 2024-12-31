using System;

namespace LostKaiju.Models.FSM.FiniteTransitions
{
    public interface IFiniteTransition
    {
        public Type FromStateType { get; }
        public Type ToStateType { get; }
        public Func<bool> Condition { get; }
    }
}
