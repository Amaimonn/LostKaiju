using UnityEngine;
using R3;

using LostKaiju.Gameplay.Creatures.DamageSystem;
using LostKaiju.Gameplay.Creatures.Features;
using LostKaiju.Gameplay.Creatures.Presenters;
using LostKaiju.Gameplay.Creatures.Views;
using LostKaiju.Gameplay.Player.Data;

namespace LostKaiju.Gameplay.Player.Behaviour
{
    public class PlayerDefencePresenter : CreaturePresenter
    {
        private Health _health;
        private DamageReceiver _damageReceiver;

        public PlayerDefencePresenter(PlayerDefenceData playerDefenceData)
        {
            _health = new Health(playerDefenceData.MaxHealth);
        }

        public override void Bind(CreatureBinder creature)
        {
            base.Bind(creature);

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