using System;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Campaign;
using System.Threading.Tasks;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        private readonly Subject<Unit> _exitSubject;
        private readonly Func<Task<CampaignModel>> _campaignModelFactory;
        private readonly IRootUIBinder _rootUIBinder;
        private bool _isMissionsOpened = false;
        private const string CAMPAIGN_NAVIGATION_VIEW_PATH = "UI/Hub/CampaignNavigationView";
        
        public HubViewModel(Subject<Unit> exitSubject, Func<Task<CampaignModel>> campaignModelFactory, 
            IRootUIBinder rootUIBinder)
        {
            _exitSubject = exitSubject;
            _campaignModelFactory = campaignModelFactory;
            _rootUIBinder = rootUIBinder;
        }

        public void OpenMissions()
        {
            if (_isMissionsOpened)
                return;

            var campaignViewPrefab = Resources.Load<CampaignNavigationView>(CAMPAIGN_NAVIGATION_VIEW_PATH);
            var campaignView = UnityEngine.Object.Instantiate(campaignViewPrefab);

            var campaignViewModel = new CampaignNavigationViewModel(_exitSubject);
            campaignViewModel.OnClosingCompleted.Subscribe(_ =>  {
                _rootUIBinder.ClearView(campaignView);
                _isMissionsOpened = false;
            });
            BindCampaignAsync();
            campaignView.Bind(campaignViewModel);
            _rootUIBinder.AddView(campaignView);
            campaignViewModel.Open();
            _isMissionsOpened = true;

            async void BindCampaignAsync()
            {
                var campaignModel = await _campaignModelFactory();
                if (campaignViewModel != null && campaignView != null)
                {
                    campaignViewModel.Bind(campaignModel);
                }
            }
        }
    }
}