using System;
using System.Collections.Generic;

using LostKaiju.Game.GameData.Campaign.Locations;

namespace LostKaiju.Game.GameData.Campaign
{
    [Serializable]
    public class CampaignState
    {
        public List<LocationState> Locations;
        public string SelectedLocationId;
        public string SelectedMissionId;
        public string LastLaunchedLocationId;
        public string LastLaunchedMissionId;
    }
}