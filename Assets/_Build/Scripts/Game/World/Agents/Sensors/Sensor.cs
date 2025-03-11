using R3;

namespace LostKaiju.Game.World.Agents.Sensors
{
    public interface ISensor<T>
    {
        public Observable<T> Detected { get; }
    }
}
