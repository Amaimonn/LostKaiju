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
        public readonly ObservableDictionary<string, LocationModel> AvailableLocationsMap;
        public readonly IReadOnlyDictionary<string, ILocationData> LocationsDataMap;
        public readonly ReactiveProperty<ILocationData> LastLaunchedLocation = new();
        public readonly ReactiveProperty<IMissionData> LastLaunchedMission = new();
        public Subject<Unit> OnStateChanged = new();
        /// <summary>
        /// all Locations config data in campaign
        /// </summary>
        private readonly IAllLocationsData _allLocationsData;

        public CampaignModel(CampaignState campaignState, IAllLocationsData allLocationsData,
            ILocationData selectedLocationData = null, IMissionData selectedMissionData = null)
            : base(campaignState)
        {
            _allLocationsData = allLocationsData;

            var locationsPairs = allLocationsData.AllData
                .Select(x => new KeyValuePair<string, ILocationData>(x.Id, x));
            LocationsDataMap = new Dictionary<string, ILocationData>(locationsPairs);

            AvailableLocationsMap = new ObservableDictionary<string, LocationModel>();
            foreach (var location in campaignState.Locations)
            {
                if (LocationsDataMap.TryGetValue(location.Id, out var locationData))
                {
                    var locationModel = new LocationModel(location, locationData);
                    AvailableLocationsMap.Add(location.Id, locationModel);
                }
                else
                {
                    Debug.LogWarning($"Location {location.Id} from state not found in locationData map");
                }
            }
            AvailableLocationsMap.ObserveAdd().Subscribe(x => State.Locations.Add(x.Value.Value.State));

            if (!String.IsNullOrEmpty(campaignState.LastLaunchedLocationId))
            {
                // last launched data
                if (LocationsDataMap.TryGetValue(campaignState.LastLaunchedLocationId, out var lastLaunchedLocationData))
                {
                    LastLaunchedLocation.Value = lastLaunchedLocationData;
                    var lastLaunchedMissionData = lastLaunchedLocationData.AllMissionsData
                        .FirstOrDefault(x => x.Id == campaignState.LastLaunchedMissionId);
                    LastLaunchedMission.Value = lastLaunchedMissionData;
                }
            }
            LastLaunchedLocation.Skip(1).Subscribe(x => State.LastLaunchedLocationId = x?.Id);
            LastLaunchedMission.Skip(1).Subscribe(x => State.LastLaunchedMissionId = x?.Id);

            if (selectedLocationData != null)
            {
                // from constructor
                SelectedLocation = new ReactiveProperty<ILocationData>(selectedLocationData);
                SelectedMission = new ReactiveProperty<IMissionData>(selectedMissionData);
            }
            else if (LastLaunchedLocation.Value != null)
            {
                SelectedLocation = new ReactiveProperty<ILocationData>(LastLaunchedLocation.Value);
                if (LastLaunchedMission.Value != null)
                    SelectedMission = new ReactiveProperty<IMissionData>(LastLaunchedMission.Value);
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

        public Subject<Unit> CreateMissionCompletionSignal(ILocationData locationData, IMissionData missionData, 
            Action saveAction)
        {
            var locationId = locationData.Id;
            var missionId =  missionData.Id;
            var currentLocationState = State.Locations.First(x => x.Id == locationId);
            var currentMissionState = currentLocationState.OpenedMissions.First(x => x.Id == missionId);
            
            var actions = new List<Action>();
            var dispatcher = new Subject<Unit>();

            if (currentMissionState.IsCompleted) // already completed, no actions needed
                return dispatcher;

            dispatcher.Subscribe(_ =>
            {
                foreach (var action in actions)
                    action?.Invoke();
            });

            actions.Add(() => currentMissionState.IsCompleted = true); // mission completed

            var currentLocationMissionsData = locationData.AllMissionsData;
            var currentMissionIndex = Array.FindIndex(currentLocationMissionsData,
                x => x.Id == missionId);

            if (currentMissionIndex < currentLocationMissionsData.Length) // open next mission
            {
                var nextMissionData = currentLocationMissionsData[currentMissionIndex + 1];
                var nextMissionState = new MissionState(nextMissionData.Id, false);
                actions.Add(() => 
                {
                    State.Locations.First(x => x.Id == locationId).OpenedMissions
                        .Add(nextMissionState);
                });
            }
            else // open next location (+ mission)
            {
                actions.Add(() => currentLocationState.IsCompleted = true);
                var currentLocationIndex = Array.FindIndex(_allLocationsData.AllData, x => x.Id == locationId);
                if (currentLocationIndex < _allLocationsData.AllData.Length - 1)
                {
                    var nextLocationData = _allLocationsData.AllData[currentLocationIndex + 1];
                    var firstMissionState = new MissionState(nextLocationData.AllMissionsData[0].Id, false);
                    var nextLocationOpenedMissions = new List<MissionState>() { firstMissionState };
                    var nextLocationState = new LocationState(nextLocationData.Id, isCompleted: false,
                        nextLocationOpenedMissions);
                    actions.Add(() => 
                    {
                        State.Locations.Add(nextLocationState);
                    });
                }
            }

            if (String.IsNullOrEmpty(currentLocationState.MaxCompletedMissionId))
            {
                actions.Add(() => currentLocationState.MaxCompletedMissionId = missionId);
            }
            else
            {
                var maxMissionIndex = Array.FindIndex(currentLocationMissionsData, 
                    x => x.Id == currentLocationState.MaxCompletedMissionId);
                if (maxMissionIndex < currentMissionIndex)
                {
                    actions.Add(() => currentLocationState.MaxCompletedMissionId = missionId);
                }
            }

            actions.Add(saveAction);

            return dispatcher;
        }

        // public void CompleteMission(ILocationData locationData, IMissionData missionData)
        // {
        //     var currentLocationModel = AvailableLocations.First(x => x.Data.Id == locationData.Id);
        //     var currentLocationMissionsData = currentLocationModel.Data.AllMissionsData;
        //     var currentMissionModel = currentLocationModel.AvailableMissionsMap[missionData.Id];
        //     currentMissionModel.IsCompleted.Value = true; // mission completed
        //     var currentMissionIndex = Array.FindIndex(currentLocationMissionsData,
        //         x => x.Id == missionData.Id);

        //     if (currentMissionIndex < currentLocationMissionsData.Length) // open next mission
        //     {
        //         var nextMissionData = currentLocationMissionsData[currentMissionIndex + 1];
        //         var nextMissionState = new MissionState(nextMissionData.Id, false);
        //         var nextMissionModel = new MissionModel(nextMissionState, nextMissionData);
        //         currentLocationModel.AvailableMissionsMap.Add(nextMissionData.Id, nextMissionModel);
        //     }
        //     else // open next location (+ mission)
        //     {
        //         var currentLocationIndex = Array.FindIndex(_allLocationsData.AllData, x => x.Id == locationData.Id);
        //         if (currentLocationIndex < _allLocationsData.AllData.Length)
        //         {
        //             var nextLocationData = _allLocationsData.AllData[currentLocationIndex + 1];
        //             var firstMissionState = new MissionState(nextLocationData.AllMissionsData[0].Id, false);
        //             var nextLocationOpenedMissions = new List<MissionState>() { firstMissionState };
        //             var nextLocationState = new LocationState(nextLocationData.Id, false, nextLocationOpenedMissions);
        //             var nextLocationModel = new LocationModel(nextLocationState, nextLocationData);
        //             AvailableLocations.Add(nextLocationModel);
        //         }
        //     }
        // }

        public void UpdateAvailableLocationsAndMissions()
        {
            bool stateChanged = false;

            foreach (var locationData in _allLocationsData.AllData)
            {
                var locationState = State.Locations.FirstOrDefault(x => x.Id == locationData.Id);

                if (locationState == null)
                {
                    var previousLocationIndex = Array.FindIndex(_allLocationsData.AllData, x => x.Id == locationData.Id) - 1;
                    var previousLocationData = _allLocationsData.AllData.ElementAtOrDefault(previousLocationIndex);

                    if (previousLocationData != null)
                    {
                        var previousLocationState = State.Locations.FirstOrDefault(x => x.Id == previousLocationData.Id);
                        if (previousLocationState != null && previousLocationState.IsCompleted)
                        {
                            var firstMissionState = new MissionState(locationData.AllMissionsData[0].Id, false);
                            var newLocationState = new LocationState(locationData.Id, false, new List<MissionState> { firstMissionState });

                            var newLocationModel = new LocationModel(newLocationState, locationData);
                            AvailableLocationsMap.Add(locationData.Id, newLocationModel);

                            stateChanged = true;
                        }
                    }
                    else if (_allLocationsData.AllData[0].Id == locationData.Id)
                    {
                        var firstMissionState = new MissionState(locationData.AllMissionsData[0].Id, false);
                        var newLocationState = new LocationState(locationData.Id, false, new List<MissionState> { firstMissionState });

                        var newLocationModel = new LocationModel(newLocationState, locationData);
                        AvailableLocationsMap.Add(locationData.Id, newLocationModel);

                        stateChanged = true;
                    }
                }
            }

            foreach (var locationState in State.Locations)
            {
                var locationData = LocationsDataMap[locationState.Id];

                if (!String.IsNullOrEmpty(locationState.MaxCompletedMissionId))
                {
                    var lastCompletedMissionIndex = Array.FindIndex(locationData.AllMissionsData, x => x.Id == locationState.MaxCompletedMissionId);

                    for (int i = 0; i <= lastCompletedMissionIndex + 1; i++)
                    {
                        if (i < locationData.AllMissionsData.Length)
                        {
                            var missionData = locationData.AllMissionsData[i];
                            var missionState = locationState.OpenedMissions.FirstOrDefault(x => x.Id == missionData.Id);

                            if (missionState == null)
                            {
                                var newMissionState = new MissionState(missionData.Id, false);
                                var locationModel = AvailableLocationsMap[locationData.Id];
                                var newMissionModel = new MissionModel(newMissionState, missionData);
                                locationModel.AvailableMissionsMap.Add(missionData.Id, newMissionModel);

                                stateChanged = true;
                            }
                        }
                    }
                }
            }

            if (stateChanged)
            {
                OnStateChanged.OnNext(Unit.Default);
            }
        }
    }
}