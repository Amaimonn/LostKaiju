using System;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    [Serializable]
    public class MissionState
    {
        public string Id;
        public bool IsOpened = false;
        public bool IsCompleted = false;

        public MissionState(string id, bool isOpened, bool isCompleted)
        {
            Id = id;
            IsOpened = isOpened;
            IsCompleted = isCompleted;
        }
    }
}