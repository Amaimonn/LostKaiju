using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Features
{
    public abstract class Flipper : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsLooksToTheRight { get; }
        public abstract void LookRight(bool isTrue);
    }
}