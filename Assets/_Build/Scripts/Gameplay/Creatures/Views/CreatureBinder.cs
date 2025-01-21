using UnityEngine;

using LostKaiju.Utils;
using LostKaiju.Game.Creatures.Features;

namespace LostKaiju.Game.Creatures.Views
{
    public abstract class CreatureBinder : MonoBehaviour, ICreatureBinder
    {
#region ICreatureBinder
        [field: SerializeField] public Rigidbody2D Rigidbody { get; protected set; }
        [field: SerializeField] public Animator Animator { get; protected set; }
        
        public Holder<ICreatureFeature> Features => _features;
#endregion

        protected Holder<ICreatureFeature> _features = new();
    }
}