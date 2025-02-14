using System.Linq;
using UnityEngine;
using R3;
using ObservableCollections;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Campaign.Locations;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class CampaignNavigationViewModel : IViewModel
    {
        public Observable<bool> OnOpenStateChanged => _isOpened;
        public Observable<Unit> OnClosingCompleted => _closingCompletedSignal;
        public ReadOnlyReactiveProperty<ILocationData> SelectedLocation => _selectedLocation;
        public ReadOnlyReactiveProperty<IMissionData> SelectedMission => _selectedMission;
        public IReadOnlyObservableList<IMissionData> DisplayedMissionsData => _displayedMissionsData;
        public IReadOnlyObservableDictionary<string, MissionModel> AvailableMissionsMap => _availableMissionsMap;

        private readonly CampaignModel _campaignModel;
        private readonly Subject<Unit> _startMissionSubject;
        private readonly ReactiveProperty<bool> _isOpened = new(false);
        private readonly Subject<Unit> _closingCompletedSignal = new();
        private readonly ReactiveProperty<ILocationData> _selectedLocation;
        private readonly ReactiveProperty<IMissionData> _selectedMission;
        private readonly ObservableList<IMissionData> _displayedMissionsData;
        private readonly ObservableDictionary<string, MissionModel> _availableMissionsMap;
        
        public CampaignNavigationViewModel(Subject<Unit> startMissionSubject, CampaignModel campaignModel)
        {
            _startMissionSubject = startMissionSubject;
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
        }

        public void StartGameplay()
        {
            Debug.Log("vm start gameplay");
            _startMissionSubject.OnNext(Unit.Default);
        }

        /// <summary>
        /// Complete closing when animation is finished. Used by View.
        /// </summary>
        public void CompleteClosing()
        {
            _closingCompletedSignal.OnNext(Unit.Default);
        }

        public void Open()
        {
            _isOpened.Value = true;
        }

        public void StartClosing()
        {
            _isOpened.Value = false;
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