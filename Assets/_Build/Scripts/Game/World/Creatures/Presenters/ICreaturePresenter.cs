using LostKaiju.Game.World.Creatures.Views;

namespace LostKaiju.Game.World.Creatures.Presenters
{
    public interface ICreaturePresenter
    {
        public void Bind(ICreatureBinder creature);
    }
}