using R3;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    public class MissionModel
    {
        public ReactiveProperty<bool> IsOpened { get; }
        public Observable<bool> IsCompleted { get; }
        public MissionState State { get; }
        public MissionData Data { get; } // additional unchangeable data for Views

        public MissionModel(MissionState state, MissionData data)
        {
            State = state;
            Data = data;

            IsOpened = new ReactiveProperty<bool>(state.IsOpened);
            IsOpened.Skip(1).Subscribe(x => State.IsOpened = x);

            IsCompleted = new ReactiveProperty<bool>(state.IsCompleted);
            IsCompleted.Skip(1).Subscribe(x => State.IsCompleted = x);
        }
    }
}