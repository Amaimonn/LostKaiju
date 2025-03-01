using LostKaiju.Game.World.Creatures.Views;

namespace LostKaiju.Game.World.Creatures.Presenters
{
    public abstract class CreaturePresenter: ICreaturePresenter
    {
        protected ICreatureBinder _creature;

        public virtual void Bind(ICreatureBinder creature)
        {
            _creature = creature;
        }
    }
}
