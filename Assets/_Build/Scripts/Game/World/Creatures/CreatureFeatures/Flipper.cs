using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public abstract class Flipper : MonoBehaviour, IFlipper
    {
        public abstract bool IsLookingToTheRight { get; }
        public abstract void LookRight(bool isTrue);
    }
}