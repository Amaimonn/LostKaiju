using UnityEngine;
using ObservableCollections;

namespace LostKaiju.Game.World.Agents.Sensors
{
    public abstract class OccludableMultiSensor<T> : Searcher<T>, IMultiSensor<T> where T : Component
    {
        public IObservableCollection<T> DetectedSet => throw new System.NotImplementedException();
        protected ObservableHashSet<T> _detectedSet = new();

        protected override void OnTargetFound(T target)
        {
            _detectedSet.Add(target);
        }

        protected override void OnTargetLost(T target)
        {
            _detectedSet.Remove(target);
        }
    }
}