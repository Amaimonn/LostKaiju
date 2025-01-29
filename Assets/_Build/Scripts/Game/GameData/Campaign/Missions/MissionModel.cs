using R3;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    public class MissionModel : Model<MissionState>
    {
        public ReactiveProperty<bool> IsOpened { get; }
        public ReactiveProperty<bool> IsCompleted { get; }
        public MissionData Data { get; } // additional unchangeable data for Views

        public MissionModel(MissionState missionState, MissionData data) : base(missionState)
        {
            Data = data;

            IsOpened = new ReactiveProperty<bool>(State.IsOpened);
            IsOpened.Skip(1).Subscribe(x => State.IsOpened = x);

            IsCompleted = new ReactiveProperty<bool>(State.IsCompleted);
            IsCompleted.Skip(1).Subscribe(x => State.IsCompleted = x);
        }
    }
}