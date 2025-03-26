using R3;

using LostKaiju.Game.World.Creatures.DamageSystem;

namespace LostKaiju.Game.GameData.HealthSystem
{
    public class HealthModel : Model<HealthState>
    {
        public Observable<int> MaxHealth => _maxHealth;
        public Observable<int> CurrentHealth => _currentHealth;
        public Observable<bool> IsDead => _isDead;

        private readonly ReactiveProperty<int> _maxHealth;
        private readonly ReactiveProperty<int> _currentHealth;
        private readonly ReactiveProperty<bool> _isDead;
        private readonly HealthCalculator _healthCalculator;

        public HealthModel(HealthState state) : base(state)
        {
            _healthCalculator = new HealthCalculator(state.MaxHealth, state.CurrentHealth);

            _maxHealth = new ReactiveProperty<int>(_healthCalculator.Max);
            _maxHealth.Skip(1).Subscribe(x => state.MaxHealth = x);

            _currentHealth = new ReactiveProperty<int>(_healthCalculator.CurrentValue);
            _currentHealth.Skip(1).Subscribe(x => state.CurrentHealth = x);

            _isDead = new ReactiveProperty<bool>(state.CurrentHealth == 0);
        }

        public void SetNewMax(int max)
        {
            _healthCalculator.Max = max;
            _maxHealth.Value = _healthCalculator.Max;
        }

        public void SetCurrent(int current)
        {
            _healthCalculator.SetValue(current);
            _currentHealth.Value = _healthCalculator.CurrentValue;
            UpdateDeathState();
        }
        public void Revive()
        {
            RestoreFullHealth();
            UpdateDeathState();
        }

        public void RestoreHealth(int amount)
        {
            _healthCalculator.Restore(amount);
            _currentHealth.Value = _healthCalculator.CurrentValue;
        }

        public void RestoreFullHealth()
        {
            _healthCalculator.Restore(_healthCalculator.Max);
            _currentHealth.Value = _healthCalculator.CurrentValue;
        }

        public void DecreaseHealth(int amount)
        {
            _healthCalculator.Decrease(amount);
            _currentHealth.Value = _healthCalculator.CurrentValue;
            UpdateDeathState();
        }

        private void UpdateDeathState()
        {
            _isDead.Value = _currentHealth.Value == 0;
        }
    }
}