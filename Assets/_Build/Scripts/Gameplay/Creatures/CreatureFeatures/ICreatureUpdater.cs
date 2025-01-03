using LostKaiju.Gameplay.Creatures.Presenters;

namespace LostKaiju.Gameplay.Creatures.Features
 {
    public interface ICreatureUpdater : ICreatureFeature
    {
        public void SetCreaturePresenter(CreaturePresenter creaturePresenter);
    }
}
