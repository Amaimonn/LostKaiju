using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using R3;
using ObservableCollections;

using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;


namespace LostKaiju.Game.GameData.Campaign
{
    public class CampaignModel : Model<CampaignState>
    {
        public readonly ReactiveProperty<ILocationData> SelectedLocation;
        public readonly ReactiveProperty<IMissionData> SelectedMission;
        public readonly ILocationData LastLaunchedLocation;
        public readonly IMissionData LastLaunchedMission;
        /// <summary>
        /// all Locations const data in campaign
        /// </summary>
        public readonly IReadOnlyDictionary<string, ILocationData> LocationsDataMap;
        public readonly ObservableList<LocationModel> AvailableLocations;

        public CampaignModel(CampaignState campaignState, Dictionary<string, ILocationData> locationsData, 
            ILocationData selectedLocationData = null, IMissionData selectedMissionData = null)
            : base(campaignState)
        {
            LocationsDataMap = locationsData;
            AvailableLocations = new ObservableList<LocationModel>();
            foreach (var location in campaignState.Locations)
            {
                if (locationsData.TryGetValue(location.Id, out var locationData))
                {
                    var locationModel = new LocationModel(location, locationData);
                    AvailableLocations.Add(locationModel);
                }
                else
                {
                    Debug.LogWarning($"Location {location.Id} from state not found in locationData map");
                }
            }

            if (!String.IsNullOrEmpty(campaignState.LastLaunchedLocationId))
            {
                // last launched data
                locationsData.TryGetValue(campaignState.LastLaunchedLocationId, out LastLaunchedLocation);
                if (LastLaunchedLocation != null)
                {
                    LastLaunchedMission = LastLaunchedLocation.AllMissionsData
                        .FirstOrDefault(x => x.Id == campaignState.LastLaunchedMissionId);
                }
            }

            if (selectedLocationData != null)
            {
                // from constructor
                SelectedLocation = new ReactiveProperty<ILocationData>(selectedLocationData);
                SelectedMission = new ReactiveProperty<IMissionData>(selectedMissionData);
            }
            else
            {
                var locationIdToSelect = campaignState.SelectedLocationId;
                if (!String.IsNullOrEmpty(locationIdToSelect) && 
                    LocationsDataMap.TryGetValue(locationIdToSelect, out var locationDataToSelect))
                {
                    // from state
                    SelectedLocation = new ReactiveProperty<ILocationData>(locationDataToSelect);
                    var missionIdToSelect = campaignState.SelectedMissionId;
                    if (String.IsNullOrEmpty(missionIdToSelect))
                    {
                        SelectedMission = new ReactiveProperty<IMissionData>(null);
                    }
                    else
                    {
                        var missionDataToSelect = locationDataToSelect.AllMissionsData
                            .FirstOrDefault(x => x.Id == missionIdToSelect);
                        SelectedMission = new ReactiveProperty<IMissionData>(missionDataToSelect);
                    }
                }
                else
                {
                    // default
                    var firstLocationData = LocationsDataMap.First().Value;
                    SelectedLocation = new ReactiveProperty<ILocationData>(firstLocationData);
                    SelectedMission = new ReactiveProperty<IMissionData>(null);
                }
                
            }
        }
    }
}