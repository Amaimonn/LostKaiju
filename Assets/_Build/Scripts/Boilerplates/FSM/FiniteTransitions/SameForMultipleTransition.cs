using System;
using System.Collections.Generic;
using System.Linq;

namespace LostKaiju.Boilerplates.FSM.FiniteTransitions
{
    public class SameForMultipleTransition<TTo> : IFiniteTransition where TTo : FiniteState
    {
#region IFiniteTransition
        public Type ToStateType => typeof(TTo);
        public Func<bool> Condition {get;}
        public bool CheckFromStateType(Type type) => _fromTypes.Contains(type);
#endregion

        private readonly IEnumerable<Type> _fromTypes;

        public SameForMultipleTransition(Func<bool> condition, IEnumerable<Type> fromTypes)
        {
            Condition = condition;
            _fromTypes = fromTypes;
        }
    }
}
