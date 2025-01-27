using System.Collections.Generic;

using LostKaiju.Game.GameData.Campaign.Locations;

namespace LostKaiju.Game.GameData.Campaign
{
    public class CampaignState
    {
        public List<LocationState> Locations;
        public string SelectedMissionId;
    }
}