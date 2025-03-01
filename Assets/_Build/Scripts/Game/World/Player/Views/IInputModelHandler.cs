using LostKaiju.Game.World.Creatures.Presenters;

namespace LostKaiju.Game.World.Player.Views
{
    public interface IBehaviourBinder
    {
        public void Bind(ICreaturePresenter model);
    }
}
