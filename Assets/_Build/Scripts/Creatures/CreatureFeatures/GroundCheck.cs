using UnityEngine;

namespace LostKaiju.Creatures.CreatureFeatures
{
    public abstract class GroundCheck : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsGrounded { get; }
    }
} 
