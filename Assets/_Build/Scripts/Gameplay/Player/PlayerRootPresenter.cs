using LostKaiju.Gameplay.Creatures.Features;
using LostKaiju.Gameplay.Creatures.Presenters;
using LostKaiju.Gameplay.Creatures.Views;
using LostKaiju.Gameplay.Player.Behaviour;

namespace LostKaiju.Gameplay.Player
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
        public override void Bind(CreatureBinder creature)
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
