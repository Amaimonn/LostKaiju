using System.Linq;
using UnityEngine;
using R3;
using ObservableCollections;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Campaign.Locations;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class CampaignNavigationViewModel : BaseScreenViewModel
    {
        public Observable<bool> IsLoaded => _isLoaded;
        public ReadOnlyReactiveProperty<ILocationData> SelectedLocation => _selectedLocation;
        public ReadOnlyReactiveProperty<IMissionData> SelectedMission => _selectedMission;
        public IReadOnlyObservableList<IMissionData> DisplayedMissionsData => _displayedMissionsData;
        public IReadOnlyObservableDictionary<string, MissionModel> AvailableMissionsMap => _availableMissionsMap;

        private CampaignModel _campaignModel;
        private readonly Subject<Unit> _startMissionSubject;
        private readonly ReactiveProperty<bool> _isLoaded = new(false);
        private ReactiveProperty<ILocationData> _selectedLocation;
        private ReactiveProperty<IMissionData> _selectedMission;
        private ObservableList<IMissionData> _displayedMissionsData;
        private ObservableDictionary<string, MissionModel> _availableMissionsMap;
        
        public CampaignNavigationViewModel(Subject<Unit> startMissionSubject)
        {
            _startMissionSubject = startMissionSubject;
        }

        public void Bind(CampaignModel campaignModel)
        {
            _campaignModel = campaignModel;

            // test with first location
            var displayedLocationModel = campaignModel.AvailableLocations[0];
            _displayedMissionsData = new ObservableList<IMissionData>(displayedLocationModel.Data.AllMissionsData); 
            _availableMissionsMap = new ObservableDictionary<string, MissionModel>(displayedLocationModel.AvailableMissionsMap);
            
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
            _startMissionSubject.OnNext(Unit.Default);
        }

        public void SelectLocation(ILocationData locationData)
        {
            _campaignModel.SelectedLocation.Value = locationData;
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