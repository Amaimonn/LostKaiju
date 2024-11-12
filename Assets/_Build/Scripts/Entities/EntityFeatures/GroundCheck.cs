using UnityEngine;

namespace LostKaiju.Entities.EntityFeatures
{
    public abstract class GroundCheck : MonoBehaviour, IEntityFeature
    {
        public abstract bool IsGrounded { get; }
    }
}
