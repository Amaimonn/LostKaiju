using System.Collections.Generic;
using System.Linq;
using R3;
using ObservableCollections;

using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;
using UnityEngine;

namespace LostKaiju.Game.GameData.Campaign
{
    public class CampaignModel : Model<CampaignState>
    {
        public readonly ReactiveProperty<ILocationData> SelectedLocation;
        public readonly ReactiveProperty<IMissionData> SelectedMission;
        public readonly IMissionData LastLaunchedMission;
        /// <summary>
        /// all Locations const data in campaign
        /// </summary>
        public readonly IReadOnlyDictionary<string, ILocationData> LocationsDataMap;
        public readonly ObservableList<LocationModel> Locations;

        public CampaignModel(CampaignState campaignState, Dictionary<string, ILocationData> locationsData,
            IMissionData selectedMission = null) : base(campaignState)
        {
            LocationsDataMap = locationsData;
            Locations = new ObservableList<LocationModel>();
            foreach (var location in campaignState.Locations)
            {
                if (locationsData.TryGetValue(location.Id, out var locationData))
                {
                    var locationModel = new LocationModel(location, locationData);
                    Locations.Add(locationModel);
                }
                else
                {
                    Debug.LogWarning($"Location {location.Id} from state not found in locationData map");
                }
            }
            
            var missionDatas = locationsData.First().Value.AllMissionsData; // test with first location
            if (selectedMission == null && missionDatas.Count() > 0)
            {
                var baseSelectedMission = missionDatas.First();
                SelectedMission = new ReactiveProperty<IMissionData>(baseSelectedMission);
            }
            else
            {
                SelectedMission = new ReactiveProperty<IMissionData>(selectedMission);
            }
        }
    }
}