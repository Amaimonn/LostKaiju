using System;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Boilerplates.Locator;
using LostKaiju.Game.GameData.Missions;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        private Subject<Unit> _exitSubject;
        private Func<MissionsModel> _missionsModelFactory;
        private readonly IRootUIBinder _rootUIBinder;
        private bool _isMissionsOpened = false;
        private const string MISSIONS_VIEW_PATH = "UI/Hub/MissionsView";
        
        public HubViewModel(Subject<Unit> exitSubject, Func<MissionsModel> missionsModelFactory)
        {
            _exitSubject = exitSubject;
            _missionsModelFactory = missionsModelFactory;
            _rootUIBinder = ServiceLocator.Current.Get<IRootUIBinder>();
        }

        public void OpenMissions()
        {
            if (_isMissionsOpened)
            {
                return;
            }

            var missionsViewPrefab = Resources.Load<MissionsView>(MISSIONS_VIEW_PATH);
            var missionsView = UnityEngine.Object.Instantiate(missionsViewPrefab);
            var missionsModel = _missionsModelFactory();
            var missionsViewModel = new MissionsViewModel(_exitSubject, missionsModel);
            missionsViewModel.OnCloseCompleted.Subscribe(_ =>  {
                _rootUIBinder.ClearView(missionsView);
                _isMissionsOpened = false;
            });

            missionsView.Bind(missionsViewModel);
            _rootUIBinder.AddView(missionsView);
            missionsViewModel.Open();
            _isMissionsOpened = true;
        }
    }
}