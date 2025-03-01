using LostKaiju.Boilerplates.FSM;
using LostKaiju.Game.World.Agents;

namespace LostKaiju.Game.World.Enemy.EnemyStates
{
    public abstract class EnemyState: FiniteState
    {
        protected Agent _agent;

        public EnemyState(Agent agent)
        {
            _agent = agent;
        }
    }
}