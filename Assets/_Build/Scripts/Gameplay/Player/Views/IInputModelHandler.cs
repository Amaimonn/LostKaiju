using LostKaiju.Gameplay.Creatures.Presenters;

namespace LostKaiju.Gameplay.Player.Views
{
    public interface IBehaviourBinder
    {
        public void Bind(CreaturePresenter model);
    }
}
