using LostKaiju.Utils;
using LostKaiju.Gameplay.Creatures.Features;
using LostKaiju.Gameplay.Creatures.Presenters;
using LostKaiju.Gameplay.Player.Data;
using LostKaiju.Gameplay.Creatures.Views;
using LostKaiju.Gameplay.Player.Behaviour;

namespace LostKaiju.Gameplay.Player
{
    public class PlayerRootPresenter : CreaturePresenter
    {
        private readonly PlayerInputPresenter _inputPresenter;
        private readonly PlayerDefencePresenter _defencePresenter;

        public PlayerRootPresenter(PlayerControlsData controlsData, PlayerDefenceData playerDefenceData)
        {
            _inputPresenter = new PlayerInputPresenter(controlsData);
            _defencePresenter = new PlayerDefencePresenter(playerDefenceData);
        }

#region CreaturePresenter
        public override void Bind(CreatureBinder creature, Holder<ICreatureFeature> features)
        {
            base.Bind(creature, features);

            _inputPresenter.Bind(creature, features);
            _defencePresenter.Bind(creature, features);
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
