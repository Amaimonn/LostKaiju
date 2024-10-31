using UnityEngine;

public abstract class GroundCheck : MonoBehaviour, IEntityFeature
{
    public abstract bool IsGrounded { get; }
}