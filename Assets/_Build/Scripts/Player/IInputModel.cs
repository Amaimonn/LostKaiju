using System;
using UnityEngine;

[Serializable]
public abstract class InputModel
{
    public Rigidbody2D ModelRigidbody { get; protected set;}
    public Animator ModelAnimator { get; protected set;}
    public GroundCheck GroundCheck { get; protected set;}
    public Flipper Flipper { get; protected set;}

    public virtual void Bind(IPlayer player, GroundCheck groundCheck = null, Flipper flipper = null)
    {
        ModelRigidbody = player.PlayerRigidbody;
        ModelAnimator = player.PlayerAnimator;
        GroundCheck = groundCheck;
        Flipper = flipper;
    }

    public abstract void UpdateLogic();
    public abstract  void FixedUpdateLogic();
}