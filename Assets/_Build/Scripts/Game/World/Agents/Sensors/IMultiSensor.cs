using ObservableCollections;

namespace LostKaiju.Game.World.Agents.Sensors
{
    public interface IMultiSensor<T>
    {
        public IObservableCollection<T> DetectedSet { get; }
    }
}
