using LostKaiju.Game.Creatures.Views;

namespace LostKaiju.Game.Creatures.Presenters
{
    public abstract class CreaturePresenter
    {
        public ICreatureBinder Creature { get; protected set; }

        public virtual void Bind(ICreatureBinder creature)
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
