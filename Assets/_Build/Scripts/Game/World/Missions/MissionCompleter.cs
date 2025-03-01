using R3;

namespace LostKaiju.Game.World.Missions
{
    public class MissionCompleter
    {
        public Observable<Unit> OnMissionCompleted => _onMissionCompleted;
        private readonly Subject<Unit> _onMissionCompleted = new();

        public void CompleteMission()
        {
            _onMissionCompleted.OnNext(Unit.Default);
        }
    }
}