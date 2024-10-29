using System;

public interface IFiniteTransition
{
    //public FiniteState FromState {get;}
    public Type FromStateType {get;}

    //public FiniteState ToState {get;}
    public Type ToStateType {get;}
    
    public Func<bool> Condition {get;}
}
