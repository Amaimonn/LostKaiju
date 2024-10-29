using UnityEngine;

public abstract class Flipper : MonoBehaviour, IEntityFeature
{
    public abstract bool IsLooksToTheRight { get; }
    public abstract void LookRight(bool isTrue);
}