using LostKaiju.Creatures.Presenters;

namespace LostKaiju.Player.Views
{
    public interface IBehaviourBinder
    {
        public void Bind(CreaturePresenter model);
    }
}
