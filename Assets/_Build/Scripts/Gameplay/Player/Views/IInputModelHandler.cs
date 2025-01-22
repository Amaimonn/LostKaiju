using LostKaiju.Game.Creatures.Presenters;

namespace LostKaiju.Game.Player.Views
{
    public interface IBehaviourBinder
    {
        public void Bind(ICreaturePresenter model);
    }
}
