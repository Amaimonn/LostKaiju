using R3;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Player.Data;
using LostKaiju.Game.World.Player.Data.Indicators;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public class PlayerDefencePresenter : IPlayerDefencePresenter
    {
        private readonly HealthModel _healthModel;
        private IDamageReceiver _damageReceiver;

        public PlayerDefencePresenter(HealthModel healthModel, PlayerDefenceData playerDefenceData)
        {
            _healthModel = healthModel;
        }

        public void Bind(ICreatureBinder creature)
        {
            var features = creature.Features;
            _damageReceiver = features.Resolve<IDamageReceiver>();
            _damageReceiver.OnDamageTaken.Subscribe(DecreaseHealth);
        }

        private void IncreaseHealth(int amount)
        {
            _healthModel.IncreaseHealth(amount);
        }

        private void DecreaseHealth(int amount)
        {
            _healthModel.DecreaseHealth(amount);
        }
    }
}