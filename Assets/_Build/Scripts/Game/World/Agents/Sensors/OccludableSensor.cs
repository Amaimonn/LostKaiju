using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Agents.Sensors
{
    public abstract class OccludableSensor<T> : Searcher<T>, ISensor<T> where T : Component
    {
        public Observable<T> Detected => _detected;
        protected ReactiveProperty<T> _detected = new();

        protected override void OnTargetFound(T target)
        {
            _detected.Value = target;
        }

        protected override void OnTargetLost(T target)
        {
            if (_detected.Value != null && _detected.Value.Equals(target))
            {
                _detected.Value = null;
            }
        }
    }
}
