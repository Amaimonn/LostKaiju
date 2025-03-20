using R3;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Player.Data;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.World.Player.Views;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public class PlayerDefencePresenter : IPlayerDefencePresenter
    {
        public Observable<Unit> OnDeath => _onDeath;
        private readonly HealthModel _healthModel;
        private IDamageReceiver _damageReceiver;
        private readonly Subject<Unit> _onDeath = new();
        private bool _isInvincible;
        private CompositeDisposable _disposables = new();

        public PlayerDefencePresenter(HealthModel healthModel, PlayerDefenceData playerDefenceData)
        {
            _healthModel = healthModel;
            _healthModel.CurrentHealth.Where(x => x == 0)
                .Subscribe(_ => _onDeath.OnNext(Unit.Default)).AddTo(_disposables);
        }

        public void Bind(ICreatureBinder creature)
        {
            var features = creature.Features;
            _damageReceiver = features.Resolve<IDamageReceiver>();
            var damagedObserver = _damageReceiver.OnDamageTaken.Where(_ => !_isInvincible);
            if (features.TryResolve<PlayerJuicySystem>(out var juicySystem))
            {
                damagedObserver.Subscribe(x => 
                {
                    juicySystem.PlayOnDamaged();
                    DecreaseHealth(x);
                })
                .AddTo(_disposables);
            }
            else
            {
                damagedObserver.Subscribe(DecreaseHealth).AddTo(_disposables);
            }
        }

        public void SetInvincible(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }

        private void RestoreHealth(int amount)
        {
            _healthModel.RestoreHealth(amount);
        }

        private void DecreaseHealth(int amount)
        {
            _healthModel.DecreaseHealth(amount);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _disposables = null;
        }
    }
}