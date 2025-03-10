using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Enemy.EnemyStates;

namespace LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States
{
    public class AttackState : EnemyState
    {
        protected IAttacker _attacker;

        public AttackState(Agent agent, IAttacker attacker) : base(agent)
        {
            _attacker = attacker;
        }

        public override void UpdateLogic()
        {
            HandleTransitions();
        }

        public override void Enter()
        {
            base.Enter();
            _agent.Stop();
            Attack();
        }

        private void Attack()
        {
            _attacker.Attack();
        }
    }
}