using System.Collections.Generic;
using System.Linq;
using R3;
using ObservableCollections;

using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.GameData.Campaign
{
    public class CampaignModel : Model<CampaignState>
    {
        public readonly ReactiveProperty<MissionData> SelectedMission;
        public readonly IReadOnlyList<MissionData> MissionDataList;
        public readonly ObservableList<LocationModel> Locations;

        public CampaignModel(CampaignState campaignState, IEnumerable<MissionData> missionDatas, 
            MissionData selectedMission = null) : base(campaignState)
        {
            MissionDataList = new List<MissionData>(missionDatas);

            if (selectedMission == null && missionDatas.Count() > 0)
            {
                var baseSelectedMission = missionDatas.First();
                SelectedMission = new ReactiveProperty<MissionData>(baseSelectedMission);
            }
            else 
            {
                SelectedMission = new ReactiveProperty<MissionData>(selectedMission);
            }
        }
    }
}