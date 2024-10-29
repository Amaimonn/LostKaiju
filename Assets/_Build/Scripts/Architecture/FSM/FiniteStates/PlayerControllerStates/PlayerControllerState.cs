using System;
using System.Collections.Generic;

public abstract class PlayerControllerState : FiniteState
{
    protected IInputProvider InputProvider { get; }
    
    public PlayerControllerState() : base()
    {
        InputProvider = ServiceLocator.Current.Get<IInputProvider>();
    }

    public virtual void Init(Dictionary<Type, IEntityFeature> features)
    {
        
    } 
}
