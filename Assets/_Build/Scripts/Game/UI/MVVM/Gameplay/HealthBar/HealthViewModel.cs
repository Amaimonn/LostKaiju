using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.HealthSystem;


namespace LostKaiju.Game.UI.MVVM.Gameplay.EnemyCreature
{
    public class HealthViewModel : IViewModel
    {
        public Observable<int> MaxHealth => _maxHealth;
        public Observable<int> CurrentHealth => _currentHealth;
        public Observable<float> HealthFillAmount => _healthFillAmount;

        private readonly ReactiveProperty<int> _maxHealth = new();
        private readonly ReactiveProperty<int> _currentHealth = new();
        private readonly ReactiveProperty<float> _healthFillAmount = new();
        private readonly HealthModel _healthModel;

        public HealthViewModel(HealthModel healthModel)
        {
            _healthModel = healthModel;
            _healthModel.MaxHealth.Subscribe(OnMaxHealthSet);
            _healthModel.CurrentHealth.Subscribe(OnCurrentHealthSet);
        }

        private void OnMaxHealthSet(int amount)
        {
            _maxHealth.Value = amount;
        }

        private void OnCurrentHealthSet(int amount)
        {
            _currentHealth.Value = amount;
            _healthFillAmount.Value = _currentHealth.Value / (float)_maxHealth.Value;
        }
    }
}