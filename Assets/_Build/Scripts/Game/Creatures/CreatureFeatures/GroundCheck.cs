using UnityEngine;

namespace LostKaiju.Game.Creatures.Features
{
    public abstract class GroundCheck : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsGrounded { get; }
    }
} 
