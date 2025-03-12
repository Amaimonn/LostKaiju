using UnityEngine;
using R3;

using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Agents.Sensors;
using LostKaiju.Game.World.Enemy.Configs;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.UI.MVVM.Gameplay.EnemyCreature;
using LostKaiju.Game.World.Creatures.Combat.AttackSystem;

namespace LostKaiju.Game.World.Enemy
{
    public class GuardianPatrollerPresenter : MonoBehaviour, IForceable
    {
        [SerializeField] private CreatureBinder _creatureBinder;
        [SerializeField] private GroundAgent _groundAgent;
        [SerializeField] private OccludablePlayerSensor _playerSensor;
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private EnemyAttackDataSO _attackData;
        [SerializeField] private EnemyDefenceDataSO _defenceData;
        [SerializeField] private EnemyHealthWorldView _healthView;

        private PatrollerAIPresenter _patrollerAIPresenter;

#region IForceable
        public void Force(Vector2 origin, float force)
        {
            var directionVector = (Vector2)_creatureBinder.transform.position - origin;
            if (directionVector.y < 0)
                directionVector.y = 0;
            _creatureBinder.Rigidbody.AddForce(force * directionVector.normalized, ForceMode2D.Impulse);
        }
#endregion
        private void Start()
        {
            _creatureBinder.Init();
            BindAI();
            BindDefence();
        }

        private void BindAI()
        {
            _patrollerAIPresenter = new PatrollerAIPresenter(_attackData,  _groundAgent, _playerSensor, _patrolPoints);
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
            Destroy(gameObject);
        }

        private void Update()
        {
            _patrollerAIPresenter.UpdateLogic();
        }
    }
}
