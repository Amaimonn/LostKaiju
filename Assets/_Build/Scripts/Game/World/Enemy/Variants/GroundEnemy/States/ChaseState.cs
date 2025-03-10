using UnityEngine;

using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Enemy.EnemyStates;

namespace LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States
{
    public class ChaseState : EnemyState
    {
        private readonly ITargeter _targeter;
        private float _stopDistance;
        public ChaseState(Agent agent, ITargeter targeter) : base(agent)
        {
            _targeter = targeter;
        }

        public void Init(float stopDistance)
        {
            _stopDistance = stopDistance;
        }

        public override void UpdateLogic()
        {
            if (_targeter.IsTargeting)
            {
                var targetPosition = _targeter.GetTargetPosition();
                var distanceX = Mathf.Abs(targetPosition.x - _agent.transform.position.x);
                if (distanceX > _stopDistance)
                    _agent.SetDestination(targetPosition);
                else
                    _agent.LookAt(targetPosition);
                    
            }
            HandleTransitions();
        }
    }
}