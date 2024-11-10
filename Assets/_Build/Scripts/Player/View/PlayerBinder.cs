using UnityEngine;

namespace Assets._Build.Scripts.Player.View
{
    public abstract class PlayerBinder : MonoBehaviour
    {
        public abstract Rigidbody2D PlayerRigidbody { get; } 
        public abstract Animator PlayerAnimator { get; } 
    }
}
