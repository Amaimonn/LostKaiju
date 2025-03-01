using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public abstract class GroundCheck : MonoBehaviour, IGroundCheck
    {
        public abstract bool IsGrounded { get; }
    }
} 
