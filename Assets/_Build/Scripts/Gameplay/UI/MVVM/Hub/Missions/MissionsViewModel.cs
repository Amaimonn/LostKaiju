using System.Linq;
using UnityEngine;
using R3;
using ObservableCollections;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Missions;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class MissionsViewModel : IViewModel//, IDisposable
    {
        public Observable<bool> OnOpenStateChanged => _isOpened;
        public Observable<Unit> OnCloseCompleted => _closeCompleted;
        public ReadOnlyReactiveProperty<MissionData> SelectedMission => _selectedMission;
        public IObservableCollection<MissionData> DisplayedMissions => _dysplayedMissions;

        private readonly MissionsModel _missionsModel;
        private readonly Subject<Unit> _startMissionSubject;
        private readonly ReactiveProperty<bool> _isOpened = new(false);
        private readonly Subject<Unit> _closeCompleted = new();
        private readonly ReactiveProperty<MissionData> _selectedMission;
        private readonly ObservableList<MissionData> _dysplayedMissions;
        // private CompositeDisposable _disposables;
        
        public MissionsViewModel(Subject<Unit> startMissionSubject, MissionsModel missionsModel)
        {
            _startMissionSubject = startMissionSubject;

            _missionsModel = missionsModel;
            var missionDataList = missionsModel.MissionDataList;
            _dysplayedMissions = new ObservableList<MissionData>(missionDataList);
            _selectedMission = new ReactiveProperty<MissionData>(_missionsModel.SelectedMission.Value);
            Debug.Log($"init in vm: {_selectedMission.Value}");
            _missionsModel.SelectedMission.Skip(1).Subscribe(OnMissionSelected);
            // _disposables = new()
            // {
            //     _missionsModel.SelectedMission.Subscribe(OnMissionSelected)
            // };
        }

        public void StartGameplay()
        {
            Debug.Log("vm start gameplay");
            _startMissionSubject.OnNext(Unit.Default);
        }

        public void CompleteClose()
        {
            _closeCompleted.OnNext(Unit.Default);
        }

        public void Open()
        {
            _isOpened.Value = true;
        }

        public void Close()
        {
            _isOpened.Value = false;
        }

        public void SelectMission(MissionData missionData)
        {
            _missionsModel.SelectedMission.Value = missionData;
        }

        private void OnMissionSelected(MissionData selectedMissionData)
        {
            _selectedMission.Value = selectedMissionData;
        }

        // public void Dispose()
        // {
        //     _disposables.Dispose();
        // }
    }
}