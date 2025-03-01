using R3;
using UnityEngine;

namespace LostKaiju.Game.World.Agents
{
    public abstract class Agent : MonoBehaviour
    {
        public Vector3 Destination { get; protected set; }
        public ReadOnlyReactiveProperty<bool> IsStopped => _isStopped;

        protected ReactiveProperty<bool> _isStopped = new(true);
        
        public abstract void SetDestination(Vector3 point);
    }
}
