using UnityEngine;
using R3;

using LostKaiju.Game.World.Enemy.Configs;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.UI.MVVM.Gameplay.EnemyCreature;
using LostKaiju.Game.World.Creatures.Combat.AttackSystem;

namespace LostKaiju.Game.World.Enemy
{
    public class SharpStonePresenter : EnemyRootPresenter
    {
        [SerializeField] private CreatureBinder _creatureBinder;
        // [SerializeField] private EnemyAttackDataSO _attackData;
        [SerializeField] private EnemyDefenceDataSO _defenceData;
        [SerializeField] private EnemyHealthWorldView _healthView;
        [SerializeField] private GameObject _toDisableOnDeath;
        [SerializeField] private GameObject _toDestory;
        private EnemyJuicySystem _juicySystem;

        public override void Init()
        {
            _creatureBinder.Init();
            if (_creatureBinder.Features.TryResolve<EnemyJuicySystem>(out _juicySystem))
                _juicySystem.Construct(_audioPlayer);
            if (_juicySystem != null && _creatureBinder.Features.TryResolve<DamagingCollision>(out var damagingCollision))
            {
                damagingCollision.OnTargetAttacked.Subscribe(_ => _juicySystem.PlayAttack()).AddTo(this);
            }

            BindDefence();
        }

        private void BindDefence()
        {
            var healthState = new HealthState(_defenceData.MaxHealth);
            var healthModel = new HealthModel(healthState);
            var defencePresenter = new EnemyDefencePresenter(healthModel, _defenceData);
            defencePresenter.Bind(_creatureBinder);
            defencePresenter.OnDeath.Take(1).Subscribe(_ => OnDeath());
            var healthViewModel = new HealthViewModel(healthModel);
            _healthView.Bind(healthViewModel);
        }

        private void OnDeath()
        {
            _toDisableOnDeath.SetActive(false);
            enabled = false;
            Destroy(_toDestory, 0.5f);
        }
    }
}
