using System;
using UnityEngine;

public abstract class CharacterBehaviour
{
    public Rigidbody2D CharacterRigidbody { get; protected set;}
    public Animator CharacterAnimator { get; protected set;}
    public Holder<IEntityFeature> Features { get; protected set;}

    public virtual void Bind(Rigidbody2D rigidbody, Animator animator, Holder<IEntityFeature> features)
    {
        CharacterRigidbody = rigidbody;
        CharacterAnimator = animator;
        Features = features;
    }

    public virtual void UpdateLogic()
    {
    }

    public virtual void FixedUpdateLogic()
    {
    }
}