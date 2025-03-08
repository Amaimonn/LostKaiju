using System;
using System.Collections.Generic;

using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    [Serializable]
    public class LocationState
    {
        public string Id;
        public bool IsCompleted;
        public List<MissionState> OpenedMissions;
        public string MaxCompletedMissionId;

        public LocationState(string id, bool isCompleted, List<MissionState> openedMissions)
        {
            Id = id;
            IsCompleted = isCompleted;
            OpenedMissions = openedMissions;
        }
    }
}