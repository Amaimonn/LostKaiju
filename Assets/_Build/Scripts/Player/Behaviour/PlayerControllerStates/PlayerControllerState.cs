using System;
using System.Collections.Generic;

using Assets._Build.Scripts.Architecture.FSM;
using Assets._Build.Scripts.Architecture.Providers;
using Assets._Build.Scripts.Architecture.Services;
using Assets._Build.Scripts.EntityFeatures;

namespace Assets._Build.Scripts.Player.Behaviour.PlayerControllerStates
{
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
}
