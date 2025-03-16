using System.Linq;
using UnityEngine;
using R3;
using ObservableCollections;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Campaign.Locations;
using System.Collections.Generic;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class CampaignNavigationViewModel : ScreenViewModel
    {
        public Observable<bool> IsLoaded => _isLoaded;
        public ReadOnlyReactiveProperty<ILocationData> SelectedLocation => _selectedLocation;
        public ReadOnlyReactiveProperty<IMissionData> SelectedMission => _selectedMission;
        public IReadOnlyObservableList<ILocationData> DisplayedLocationsData => _displayedLocationsData;
        public Observable<IMissionData[]> DisplayedMissionsData => _displayedMissionsData;
        public IReadOnlyObservableDictionary<string, LocationModel> AvailableLocationsMap => _availableLocationsMap;
        public IReadOnlyObservableDictionary<string, MissionModel> AvailableMissionsMap => _availableMissionsMap;

        private CampaignModel _campaignModel;
        private readonly Subject<Unit> _startMissionSubject;
        private readonly ReactiveProperty<bool> _isLoaded = new(false);
        private ReactiveProperty<ILocationData> _selectedLocation;
        private ReactiveProperty<IMissionData> _selectedMission;
        private ObservableList<ILocationData> _displayedLocationsData;
        private ReactiveProperty<IMissionData[]> _displayedMissionsData;
        private ObservableDictionary<string, MissionModel> _availableMissionsMap;
        private ObservableDictionary<string, LocationModel> _availableLocationsMap;
        
        public CampaignNavigationViewModel(Subject<Unit> startMissionSubject)
        {
            _startMissionSubject = startMissionSubject;
        }

        public void Bind(CampaignModel campaignModel)
        {
            _campaignModel = campaignModel;

            // test with first location
            _displayedLocationsData = new ObservableList<ILocationData>(campaignModel.LocationsDataMap.Values);
            _availableLocationsMap = campaignModel.AvailableLocationsMap;
            var displayedLocationModel = campaignModel.AvailableLocationsMap[_campaignModel.SelectedLocation.Value.Id];
            _displayedMissionsData = new ReactiveProperty<IMissionData[]>(displayedLocationModel.Data.AllMissionsData); 
            var availableMissionsMap = new Dictionary<string, MissionModel>();
            foreach (var locationModel in campaignModel.AvailableLocationsMap)
            {
                foreach (var missionMapPair in locationModel.Value.AvailableMissionsMap)
                {
                    availableMissionsMap.Add(missionMapPair.Key, missionMapPair.Value);
                }
            }
            _availableMissionsMap = new ObservableDictionary<string, MissionModel>(availableMissionsMap);
            
            _selectedLocation = new ReactiveProperty<ILocationData>(_campaignModel.SelectedLocation.Value);
            _selectedMission = new ReactiveProperty<IMissionData>(_campaignModel.SelectedMission.Value);
            
            Debug.Log($"init in vm: {_selectedMission.Value}");

            _campaignModel.SelectedLocation.Skip(1).Subscribe(OnLocationSelected);
            _campaignModel.SelectedMission.Skip(1).Subscribe(OnMissionSelected);

            _isLoaded.Value = true;
        }

        public void StartGameplay()
        {
            Debug.Log("vm start gameplay");
            _campaignModel.LastLaunchedLocation.Value = _campaignModel.SelectedLocation.Value;
            _campaignModel.LastLaunchedMission.Value = _campaignModel.SelectedMission.Value;
            _startMissionSubject.OnNext(Unit.Default);
        }

        public void SelectLocation(ILocationData locationData)
        {
            _campaignModel.SelectedLocation.Value = locationData;
            if (_campaignModel.AvailableLocationsMap.TryGetValue(locationData.Id, out var locationModel))
            {
                if (locationModel.AvailableMissionsMap.Count != 0)
                    SelectMission(locationModel.AvailableMissionsMap.Last().Value.Data);
                else
                    SelectMission(null);
            }

            _displayedMissionsData.Value = _campaignModel.LocationsDataMap[locationData.Id].AllMissionsData;
        }

        public void SelectMission(IMissionData missionData)
        {
            _campaignModel.SelectedMission.Value = missionData;
        }

        private void OnLocationSelected(ILocationData selectedLocationData)
        {
            _selectedLocation.Value = selectedLocationData;
        }

        private void OnMissionSelected(IMissionData selectedMissionData)
        {
            _selectedMission.Value = selectedMissionData;
        }
    }
}