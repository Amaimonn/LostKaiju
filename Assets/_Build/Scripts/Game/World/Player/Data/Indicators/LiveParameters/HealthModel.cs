using R3;

using LostKaiju.Game.World.Creatures.DamageSystem;
using LostKaiju.Game.GameData;

namespace LostKaiju.Game.World.Player.Data.Indicators
{
    public class HealthModel : Model<HealthState>
    {
        public Observable<int> MaxHealth => _maxHealth;
        public Observable<int> CurrentHealth => _currentHealth;

        private readonly ReactiveProperty<int> _maxHealth;
        private readonly ReactiveProperty<int> _currentHealth;
        private readonly Health _health;

        public HealthModel(HealthState state) : base(state)
        {
            _health = new Health(state.MaxHealth, state.CurrentHealth);

            _maxHealth = new ReactiveProperty<int>(_health.Max);
            _maxHealth.Skip(1).Subscribe(x => state.MaxHealth = x);

            _currentHealth = new ReactiveProperty<int>(_health.CurrentValue);
            _currentHealth.Skip(1).Subscribe(x => state.CurrentHealth = x);
        }

        public void IncreaseHealth(int amount)
        {
            _health.Increase(amount);
            _currentHealth.Value = _health.CurrentValue;
        }

        public void DecreaseHealth(int amount)
        {
            _health.Decrease(amount);
            _currentHealth.Value = _health.CurrentValue;
        }
    }
}