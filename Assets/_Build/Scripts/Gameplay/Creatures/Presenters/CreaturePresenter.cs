using LostKaiju.Game.Creatures.Views;

namespace LostKaiju.Game.Creatures.Presenters
{
    public abstract class CreaturePresenter
    {
        public CreatureBinder Creature { get; protected set; }

        public virtual void Bind(CreatureBinder creature)
        {
            Creature = creature;
        }

        public virtual void UpdateLogic()
        {
        }

        public virtual void FixedUpdateLogic()
        {
        }
    }
}
