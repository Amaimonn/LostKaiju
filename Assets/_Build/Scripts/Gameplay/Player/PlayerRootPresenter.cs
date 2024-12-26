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
        private readonly PlayerInputPresenter _inputController;

        public PlayerRootPresenter(PlayerControlsData controlsData)
        {
            _inputController = new PlayerInputPresenter(controlsData);
        }

#region CreaturePresenter
        public override void Bind(CreatureBinder creature, Holder<ICreatureFeature> features)
        {
            base.Bind(creature, features);

            _inputController.Bind(creature, features);
        }
        
        public override void UpdateLogic()
        {
            _inputController.UpdateLogic();
        }

        public override void FixedUpdateLogic()
        {
            _inputController.FixedUpdateLogic();
        }
#endregion
    }
}
