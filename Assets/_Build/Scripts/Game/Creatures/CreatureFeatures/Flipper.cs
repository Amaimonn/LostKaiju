using UnityEngine;

namespace LostKaiju.Game.Creatures.Features
{
    public abstract class Flipper : MonoBehaviour, ICreatureFeature
    {
        public abstract bool IsLookingToTheRight { get; }
        public abstract void LookRight(bool isTrue);
    }
}