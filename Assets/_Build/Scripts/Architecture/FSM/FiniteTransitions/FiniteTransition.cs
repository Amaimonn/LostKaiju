using System;

public class FiniteTransition<TFrom, TTo> : IFiniteTransition
    where TFrom : FiniteState
    where TTo : FiniteState
{
    // public FiniteState FromState {get;}
    // public FiniteState ToState {get;}

    public Type FromStateType => typeof(TFrom);
    public Type ToStateType => typeof(TTo);
    
    public Func<bool> Condition {get;}

    public FiniteTransition(Func<bool> condition) // TFrom fromState, TTo toState, 
    {
        // FromState = fromState;
        // ToState = toState;
        Condition = condition;
    }
}