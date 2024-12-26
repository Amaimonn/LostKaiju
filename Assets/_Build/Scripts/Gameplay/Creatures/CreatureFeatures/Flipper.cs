using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.CreatureFeatures
{
    public abstract class Flipper : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsLooksToTheRight { get; }
        public abstract void LookRight(bool isTrue);
    }
}