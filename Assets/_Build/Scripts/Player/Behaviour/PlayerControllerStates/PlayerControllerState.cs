using System;
using System.Collections.Generic;

using LostKaiju.Architecture.FSM;
using LostKaiju.Architecture.Providers.Inputs;
using LostKaiju.Architecture.Services;
using LostKaiju.Creatures.CreatureFeatures;

namespace LostKaiju.Player.Behaviour.PlayerControllerStates
{
    public abstract class PlayerControllerState : FiniteState
    {
        protected IInputProvider InputProvider { get; }
        
        public PlayerControllerState() : base()
        {
            InputProvider = ServiceLocator.Current.Get<IInputProvider>();
        }

        public virtual void Init(Dictionary<Type, ICreatureFeature> features)
        {
            
        } 
    }
}
