using System;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    [Serializable]
    public class MissionState
    {
        public string Id;
        public bool IsCompleted = false;

        public MissionState(string id, bool isCompleted)
        {
            Id = id;
            IsCompleted = isCompleted;
        }
    }
}