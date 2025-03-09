using System;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    [Serializable]
    public class MissionState : ICopyable<MissionState>
    {
        public string Id;
        public bool IsCompleted;

        public MissionState(string id, bool isCompleted)
        {
            Id = id;
            IsCompleted = isCompleted;
        }

        public MissionState Copy()
        {
            return new MissionState(Id, IsCompleted);
        }
    }
}