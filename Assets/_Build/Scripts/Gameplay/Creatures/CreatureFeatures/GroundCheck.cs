using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Features
{
    public abstract class GroundCheck : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsGrounded { get; }
    }
} 
