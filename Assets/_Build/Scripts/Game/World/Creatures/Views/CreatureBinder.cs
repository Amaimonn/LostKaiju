using UnityEngine;
using R3;

using LostKaiju.Utils;
using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Creatures.Views
{
    public abstract class CreatureBinder : MonoBehaviour, ICreatureBinder
    {
#region ICreatureBinder
        [field: SerializeField] public Rigidbody2D Rigidbody { get; protected set; }
        [field: SerializeField] public Animator Animator { get; protected set; }
        public Observable<Unit> OnDispose => _onDispose;
        public Holder<ICreatureFeature> Features => _features;
#endregion

        protected Holder<ICreatureFeature> _features = new();
        protected Subject<Unit> _onDispose = new();

        public virtual void Init()
        {
        }
    }
}