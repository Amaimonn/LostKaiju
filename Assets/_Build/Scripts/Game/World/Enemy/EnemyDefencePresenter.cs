using R3;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.World.Enemy.Configs;

namespace LostKaiju.Game.World.Enemy
{
    public class EnemyDefencePresenter : IEnemyDefencePresenter
    {
        public Observable<Unit> OnDeath => _onDeath;
        private readonly HealthModel _healthModel;
        private IDamageReceiver _damageReceiver;
        private readonly Subject<Unit> _onDeath = new();

        public EnemyDefencePresenter(HealthModel healthModel, IEnemyDefenceData enemyDefenceData)
        {
            _healthModel = healthModel;
            _healthModel.IsDead.Where(x => x == true)
                .Subscribe(_ => _onDeath.OnNext(Unit.Default));
        }

        public void Bind(ICreatureBinder creature)
        {
            var features = creature.Features;
            _damageReceiver = features.Resolve<IDamageReceiver>();
            if (features.TryResolve<EnemyJuicySystem>(out var juicySystem))
            {
                _damageReceiver.OnDamageTaken.Subscribe(x => 
                {
                    juicySystem.PlayOnDamaged();
                    DecreaseHealth(x);
                });
            }
            else
            {
                _damageReceiver.OnDamageTaken.Subscribe(DecreaseHealth);
            }
        }

        private void RestoreHealth(int amount)
        {
            _healthModel.RestoreHealth(amount);
        }

        private void DecreaseHealth(int amount)
        {
            _healthModel.DecreaseHealth(amount);
        }
    }
}