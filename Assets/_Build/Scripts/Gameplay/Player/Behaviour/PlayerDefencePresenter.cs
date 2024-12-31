using R3;

using LostKaiju.Gameplay.Creatures.DamageSystem;
using LostKaiju.Gameplay.Creatures.Features;
using LostKaiju.Gameplay.Creatures.Presenters;
using LostKaiju.Gameplay.Creatures.Views;
using LostKaiju.Utils;

namespace LostKaiju.Gameplay.Player.Behaviour
{
    public class PlayerDefencePresenter : CreaturePresenter
    {
        private Health _health;
        private DamageReceiver _damageReceiver;

        public PlayerDefencePresenter(int maxHealth)
        {
            _health = new Health(maxHealth);
        }

        public override void Bind(CreatureBinder creature, Holder<ICreatureFeature> features)
        {
            base.Bind(creature, features);

            _damageReceiver = features.Resolve<DamageReceiver>();

            _damageReceiver.OnDamageTaken.Subscribe(DecreaseHealth);
        }

        private void DecreaseHealth(int amount)
        {
            _health.Decrease(amount);
        }
    }
}