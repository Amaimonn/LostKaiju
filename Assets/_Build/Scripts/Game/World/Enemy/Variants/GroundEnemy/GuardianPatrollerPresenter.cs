using UnityEngine;
using R3;

using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Agents.Sensors;
using LostKaiju.Game.World.Enemy.Configs;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.UI.MVVM.Gameplay.EnemyCreature;

namespace LostKaiju.Game.World.Enemy
{
    public class GuardianPatrollerPresenter : MonoBehaviour
    {
        [SerializeField] private CreatureBinder _creatureBinder;
        [SerializeField] private GroundAgent _groundAgent;
        [SerializeField] private OccludablePlayerSensor _playerSensor;
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private EnemyAttackDataSO _attackData;
        [SerializeField] private EnemyDefenceDataSO _defenceData;
        [SerializeField] private EnemyHealthWorldView _healthView;
        [SerializeField] private GameObject _toDisableOnDeath;
        [SerializeField] private GameObject _toDestory;

        private PatrollerAIPresenter _patrollerAIPresenter;

        private void Start()
        {
            _creatureBinder.Init();
            BindAI();
            BindDefence();
        }

        private void BindAI()
        {
            _patrollerAIPresenter = new PatrollerAIPresenter(_attackData, _groundAgent, _playerSensor, _patrolPoints);
            _patrollerAIPresenter.Bind(_creatureBinder);
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

        private void Update()
        {
            _patrollerAIPresenter.UpdateLogic();
        }
    }
}
