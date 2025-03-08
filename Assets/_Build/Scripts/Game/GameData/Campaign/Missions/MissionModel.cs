using R3;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    public class MissionModel : Model<MissionState>
    {
        public ReactiveProperty<bool> IsCompleted { get; }
        public IMissionData Data { get; } // additional unchangeable data for Views

        public MissionModel(MissionState missionState, IMissionData data) : base(missionState)
        {
            Data = data;

            IsCompleted = new ReactiveProperty<bool>(State.IsCompleted);
            IsCompleted.Skip(1).Subscribe(x => State.IsCompleted = x);
        }
    }
}