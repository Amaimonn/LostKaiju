using UnityEngine;

using LostKaiju.Game.Creatures.Features;
using LostKaiju.Utils;

namespace LostKaiju.Game.Creatures.Views
{
    public abstract class CreatureBinder : MonoBehaviour
    {
        public abstract Rigidbody2D Rigidbody { get; }
        public abstract Animator Animator { get; }
        public abstract Holder<ICreatureFeature> Features { get; }
    }
}
