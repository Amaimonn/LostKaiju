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
        private readonly IAllLocationsData _allLocationsData;
        public readonly ObservableList<LocationModel> AvailableLocations;

        public CampaignModel(CampaignState campaignState, IAllLocationsData allLocationsData, 
            ILocationData selectedLocationData = null, IMissionData selectedMissionData = null)
            : base(campaignState)
        {
            _allLocationsData = allLocationsData;

            var locationsPairs = allLocationsData.AllData
                .Select(x => new KeyValuePair<string, ILocationData>(x.Id, x));
            var locationsData = new Dictionary<string, ILocationData>(locationsPairs);

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
            AvailableLocations.ObserveAdd().Subscribe(x => State.Locations.Add(x.Value.State));

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

        public Action CreateMissionCompleteDispatcher(ILocationData locationData, IMissionData missionData)
        {
            Action dispatcher = () => {};
            var currentLocationState = State.Locations.First(x => x.Id == locationData.Id);
            var currentLocationMissionsData = locationData.AllMissionsData;
            var currentMissionState = currentLocationState.OpenedMissions.First(x => x.Id == missionData.Id);
            currentMissionState.IsCompleted = true; // mission completed
            var currentMissionIndex = Array.FindIndex(currentLocationMissionsData, 
                x => x.Id == missionData.Id);

            if (currentMissionIndex < currentLocationMissionsData.Length) // open next mission
            {
                var nextMissionData = currentLocationMissionsData[currentMissionIndex + 1];
                var nextMissionState = new MissionState(nextMissionData.Id, true, false);
                dispatcher = () => {
                    State.Locations.First(x => x.Id == locationData.Id).OpenedMissions
                    .Add(nextMissionState);
                    // + save
                };
            }
            else // open next location (+ mission)
            {
                var currentLocationIndex = Array.FindIndex(_allLocationsData.AllData, x => x.Id == locationData.Id);
                if (currentLocationIndex < _allLocationsData.AllData.Length)
                {
                    var nextLocationData = _allLocationsData.AllData[currentLocationIndex + 1];
                    var firstMissionState = new MissionState(nextLocationData.AllMissionsData[0].Id, true, false);
                    var nextLocationOpenedMissions = new List<MissionState>(){ firstMissionState };
                    var nextLocationState = new LocationState(nextLocationData.Id, true, nextLocationOpenedMissions);
                    dispatcher = () => {
                        State.Locations.Add(nextLocationState);
                        // + save
                    };
                }
            }
            return dispatcher;
        }
        
        public void CompleteMission(ILocationData locationData, IMissionData missionData)
        {
            var currentLocationModel = AvailableLocations.First(x => x.Data.Id == locationData.Id);
            var currentLocationMissionsData = currentLocationModel.Data.AllMissionsData;
            var currentMissionModel = currentLocationModel.AvailableMissionsMap[missionData.Id];
            currentMissionModel.IsCompleted.Value = true; // mission completed
            var currentMissionIndex = Array.FindIndex(currentLocationMissionsData, 
                x => x.Id == missionData.Id);

            if (currentMissionIndex < currentLocationMissionsData.Length) // open next mission
            {
                var nextMissionData = currentLocationMissionsData[currentMissionIndex + 1];
                var nextMissionState = new MissionState(nextMissionData.Id, true, false);
                var nextMissionModel = new MissionModel(nextMissionState, nextMissionData);
                currentLocationModel.AvailableMissionsMap.Add(nextMissionData.Id, nextMissionModel);
            }
            else // open next location (+ mission)
            {
                var currentLocationIndex = Array.FindIndex(_allLocationsData.AllData, x => x.Id == locationData.Id);
                if (currentLocationIndex < _allLocationsData.AllData.Length)
                {
                    var nextLocationData = _allLocationsData.AllData[currentLocationIndex + 1];
                    var firstMissionState = new MissionState(nextLocationData.AllMissionsData[0].Id, true, false);
                    var nextLocationOpenedMissions = new List<MissionState>(){ firstMissionState };
                    var nextLocationState = new LocationState(nextLocationData.Id, true, nextLocationOpenedMissions);
                    var nextLocationModel = new LocationModel(nextLocationState, nextLocationData);
                    AvailableLocations.Add(nextLocationModel);
                }
            }
        }
    }
}