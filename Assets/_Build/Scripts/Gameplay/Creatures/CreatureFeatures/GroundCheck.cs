using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.CreatureFeatures
{
    public abstract class GroundCheck : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsGrounded { get; }
    }
} 
