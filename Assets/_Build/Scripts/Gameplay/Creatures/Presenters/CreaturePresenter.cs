using LostKaiju.Game.Creatures.Views;

namespace LostKaiju.Game.Creatures.Presenters
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
