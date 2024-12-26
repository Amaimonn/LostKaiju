using UnityEngine;

using LostKaiju.Utils;
using LostKaiju.Gameplay.Creatures.CreatureFeatures;
using LostKaiju.Gameplay.Creatures.Views;

namespace LostKaiju.Gameplay.Creatures.Presenters
{
    public abstract class CreaturePresenter
    {
        public CreatureBinder Creature { get; protected set; }
        public Holder<ICreatureFeature> Features { get; protected set; }

        public virtual void Bind(CreatureBinder creature, Holder<ICreatureFeature> features)
        {
            Creature = creature;
            Features = features;
        }

        public virtual void UpdateLogic()
        {
        }

        public virtual void FixedUpdateLogic()
        {
        }
    }
}
