using LostKaiju.Game.World.Creatures.Presenters;

namespace LostKaiju.Game.World.Creatures.Features
 {
    public interface ICreatureUpdater : ICreatureFeature
    {
        public void SetUpdatablePresenter(IUpdatablePresenter updatablePresenter);
    }
}
