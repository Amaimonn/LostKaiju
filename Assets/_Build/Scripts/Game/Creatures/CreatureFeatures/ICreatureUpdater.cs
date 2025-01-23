using LostKaiju.Game.Creatures.Presenters;

namespace LostKaiju.Game.Creatures.Features
 {
    public interface ICreatureUpdater : ICreatureFeature
    {
        public void SetUpdatablePresenter(IUpdatablePresenter updatablePresenter);
    }
}
