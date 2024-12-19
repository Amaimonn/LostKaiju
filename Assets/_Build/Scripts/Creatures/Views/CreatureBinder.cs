using UnityEngine;

namespace LostKaiju.Creatures.Views
{
    public abstract class CreatureBinder : MonoBehaviour
    {
        public abstract Rigidbody2D Rigidbody { get; } 
        public abstract Animator Animator { get; } 
    }
}
