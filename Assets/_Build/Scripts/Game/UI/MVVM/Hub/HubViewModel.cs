using System;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Boilerplates.Locator;
using LostKaiju.Game.GameData.Campaign;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        private readonly Subject<Unit> _exitSubject;
        private readonly Func<CampaignModel> _missionsModelFactory;
        private readonly IRootUIBinder _rootUIBinder;
        private bool _isMissionsOpened = false;
        private const string CAMPAIGN_NAVIGATION_VIEW_PATH = "UI/Hub/CampaignNavigationView";
        
        public HubViewModel(Subject<Unit> exitSubject, Func<CampaignModel> missionsModelFactory)
        {
            _exitSubject = exitSubject;
            _missionsModelFactory = missionsModelFactory;
            _rootUIBinder = ServiceLocator.Instance.Get<IRootUIBinder>();
        }

        public void OpenMissions()
        {
            if (_isMissionsOpened)
            {
                return;
            }

            var missionsViewPrefab = Resources.Load<CampaignNavigationView>(CAMPAIGN_NAVIGATION_VIEW_PATH);
            var missionsView = UnityEngine.Object.Instantiate(missionsViewPrefab);
            var missionsModel = _missionsModelFactory();
            var missionsViewModel = new CampaignNavigationViewModel(_exitSubject, missionsModel);
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