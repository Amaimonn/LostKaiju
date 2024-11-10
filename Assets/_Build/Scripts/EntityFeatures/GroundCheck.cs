using UnityEngine;

namespace Assets._Build.Scripts.EntityFeatures
{
    public abstract class GroundCheck : MonoBehaviour, IEntityFeature
    {
        public abstract bool IsGrounded { get; }
    }
}
