using System;
using UnityEngine;

[Serializable]
public abstract class CharacterModel
{
    public Rigidbody2D ModelRigidbody { get; protected set;}
    public Animator ModelAnimator { get; protected set;}
    public Holder<IEntityFeature> Features { get; protected set;}

    public virtual void Bind(IPlayer player, Holder<IEntityFeature> features)
    {
        ModelRigidbody = player.PlayerRigidbody;
        ModelAnimator = player.PlayerAnimator;
        Features = features;
    }

    public abstract void UpdateLogic();
    public abstract  void FixedUpdateLogic();
}