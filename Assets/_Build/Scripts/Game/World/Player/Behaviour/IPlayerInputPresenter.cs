using LostKaiju.Game.World.Creatures.Presenters;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public interface IPlayerInputPresenter : ICreaturePresenter, IUpdatablePresenter
    {
        public void SetInputEnabled(bool isEnabled);
    }
}