using UnityEngine;
using R3;

using LostKaiju.Models.UI.MVVM;
using LostKaiju.Models.Locator;

namespace LostKaiju.Gameplay.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        private Subject<Unit> _exitSubject;
        private readonly IRootUIBinder _rootUIBinder;
        private bool _isMissionsOpened = false;
        private const string MISSIONS_VIEW_PATH = "UI/Hub/MissionsView";
        
        public HubViewModel(Subject<Unit> exitSubject)
        {
            _exitSubject = exitSubject;
            _rootUIBinder = ServiceLocator.Current.Get<IRootUIBinder>();
        }

        public void OpenMissions()
        {
            if (_isMissionsOpened)
            {
                return;
            }

            var missionsViewPrefab = Resources.Load<MissionsView>(MISSIONS_VIEW_PATH);
            var missionsView = Object.Instantiate(missionsViewPrefab);
            var missionsViewModel = new MissionsViewModel(_exitSubject);
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