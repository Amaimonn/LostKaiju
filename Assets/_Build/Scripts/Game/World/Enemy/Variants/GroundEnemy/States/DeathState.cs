using LostKaiju.Game.World.Agents;
using LostKaiju.Game.World.Enemy.EnemyStates;

namespace LostKaiju.Game.World.Enemy.Variants.GroundEnemy.States
{
    public class DeathState : EnemyState
    {
        public DeathState(Agent agent) : base(agent)
        {
        }

        public override void UpdateLogic()
        {
            HandleTransitions();
        }

        public override void Enter()
        {
            base.Enter();
            _agent.Stop();
        }
    }
}