using LostKaiju.Game.Creatures.Features;
using LostKaiju.Game.Creatures.Presenters;
using LostKaiju.Game.Creatures.Views;
using LostKaiju.Game.Player.Behaviour;

namespace LostKaiju.Game.Player
{
    public class PlayerRootPresenter : CreaturePresenter
    {
        private readonly PlayerInputPresenter _inputPresenter;
        private readonly PlayerDefencePresenter _defencePresenter;

        public PlayerRootPresenter(PlayerInputPresenter playerInputPresenter, PlayerDefencePresenter playerDefencePresenter)
        {
            _inputPresenter = playerInputPresenter;
            _defencePresenter = playerDefencePresenter;
        }

#region CreaturePresenter
        public override void Bind(ICreatureBinder creature)
        {
            base.Bind(creature);

            _inputPresenter.Bind(Creature);
            _defencePresenter.Bind(Creature);

            var features = Creature.Features;
            var creatureUpdater = features.Resolve<ICreatureUpdater>();
            creatureUpdater.SetCreaturePresenter(this);
        }
        
        public override void UpdateLogic()
        {
            _inputPresenter.UpdateLogic();
        }

        public override void FixedUpdateLogic()
        {
            _inputPresenter.FixedUpdateLogic();
        }
#endregion
    }
}
