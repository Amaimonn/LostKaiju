using R3;

using LostKaiju.Game.Creatures.Features;
using LostKaiju.Game.Creatures.Views;
using LostKaiju.Game.Player.Data;
using LostKaiju.Game.Player.Data.Indicators;

namespace LostKaiju.Game.Player.Behaviour
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