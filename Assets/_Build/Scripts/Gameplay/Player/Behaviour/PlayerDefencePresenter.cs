using UnityEngine;
using R3;

using LostKaiju.Game.Creatures.DamageSystem;
using LostKaiju.Game.Creatures.Features;
using LostKaiju.Game.Creatures.Views;
using LostKaiju.Game.Player.Data;

namespace LostKaiju.Game.Player.Behaviour
{
    public class PlayerDefencePresenter : IPlayerDefencePresenter
    {
        private Health _health;
        private DamageReceiver _damageReceiver;

        public PlayerDefencePresenter(PlayerDefenceData playerDefenceData)
        {
            _health = new Health(playerDefenceData.MaxHealth);
        }

        public void Bind(ICreatureBinder creature)
        {
            var features = creature.Features;
            _damageReceiver = features.Resolve<DamageReceiver>();
            _damageReceiver.OnDamageTaken.Subscribe(DecreaseHealth);
        }

        private void DecreaseHealth(int amount)
        {
            _health.Decrease(amount);
            Debug.Log(_health.CurrentValue);
        }
    }
}