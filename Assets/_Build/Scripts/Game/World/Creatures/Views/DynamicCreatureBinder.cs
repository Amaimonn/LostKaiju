using UnityEngine;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Creatures.Views
{
    public class DynamicCreatureBinder : CreatureBinder
    {
        [SerializeField] private Component[] _featuresToRegister;

        public override void Init()
        {
            foreach (var feature in _featuresToRegister)
            {
                if (feature is ICreatureFeature creatureFeature)
                    _features.Register(feature.GetType(), creatureFeature);
                else
                    Debug.LogError($"{feature.GetType().Name} is not a {nameof(ICreatureFeature)}.");
            }
        }
    }
}