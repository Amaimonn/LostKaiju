using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Features
{
    public abstract class Flipper : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsLookingToTheRight { get; }
        public abstract void LookRight(bool isTrue);
    }
}