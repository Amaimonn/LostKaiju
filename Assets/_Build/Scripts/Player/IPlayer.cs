using UnityEngine;

public interface IPlayer
{
    public Rigidbody2D PlayerRigidbody { get; } 
    Animator PlayerAnimator { get; } 
}
