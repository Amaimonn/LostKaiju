using LostKaiju.Game.Creatures.Views;

namespace LostKaiju.Game.Creatures.Presenters
{
    public interface ICreaturePresenter
    {
        public void Bind(ICreatureBinder creature);
    }
}