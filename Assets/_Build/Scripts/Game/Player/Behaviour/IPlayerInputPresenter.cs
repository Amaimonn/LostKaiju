using LostKaiju.Game.Creatures.Presenters;

namespace LostKaiju.Game.Player.Behaviour
{
    public interface IPlayerInputPresenter : ICreaturePresenter, IUpdatablePresenter
    {
        public void SetInputEnabled(bool isEnabled);
    }
}