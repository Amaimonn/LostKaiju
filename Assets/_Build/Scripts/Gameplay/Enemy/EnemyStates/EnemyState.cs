using LostKaiju.Boilerplates.FSM;
using LostKaiju.Game.Agents;

namespace LostKaiju.Game.Enemy.EnemyStates
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