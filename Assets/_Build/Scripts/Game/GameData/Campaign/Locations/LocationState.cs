using System.Collections.Generic;

using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    public class LocationState
    {
        public string Id;
        public bool IsOpened;
        public List<MissionState> OpenedMissions;

        public LocationState(string id, bool isOpened, List<MissionState> openedMissions)
        {
            Id = id;
            IsOpened = isOpened;
            OpenedMissions = openedMissions;
        }
    }
}