using UnityEngine;
using R3;

using LostKaiju.Game.Creatures.Features;
using LostKaiju.Utils;

namespace LostKaiju.Game.Creatures.Views
{
    public interface ICreatureBinder
    {
        public Rigidbody2D Rigidbody { get; }
        public Animator Animator { get; }
        public Holder<ICreatureFeature> Features { get; }
        public Observable<Unit> OnDispose { get; }
    }
}
