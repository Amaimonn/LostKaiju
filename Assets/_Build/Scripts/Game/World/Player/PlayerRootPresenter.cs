using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Presenters;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Player.Behaviour;

namespace LostKaiju.Game.World.Player
{
    public class PlayerRootPresenter : CreaturePresenter, IUpdatablePresenter
    {
        private readonly IPlayerInputPresenter _inputPresenter;
        private readonly IPlayerDefencePresenter _defencePresenter;

        public PlayerRootPresenter(IPlayerInputPresenter playerInputPresenter, IPlayerDefencePresenter playerDefencePresenter)
        {
            _inputPresenter = playerInputPresenter;
            _defencePresenter = playerDefencePresenter;
        }

#region CreaturePresenter
        public override void Bind(ICreatureBinder creature)
        {
            base.Bind(creature);

            _inputPresenter.Bind(_creature);
            _defencePresenter.Bind(_creature);

            var features = _creature.Features;
            var creatureUpdater = features.Resolve<ICreatureUpdater>();
            creatureUpdater.SetUpdatablePresenter(this);
        }
        
        public void UpdateLogic()
        {
            _inputPresenter.UpdateLogic();
        }

        public void FixedUpdateLogic()
        {
            _inputPresenter.FixedUpdateLogic();
        }
#endregion
    }
}
