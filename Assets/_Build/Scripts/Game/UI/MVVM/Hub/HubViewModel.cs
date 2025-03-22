using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Shared.Settings;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        public Observable<CampaignNavigationViewModel> OnCampaignOpened => _onCampaignOpened;
        
        private readonly Subject<CampaignNavigationViewModel> _onCampaignOpened = new();
        private readonly CampaignNavigationBinder _campaignNavigationBinder;
        private readonly SettingsBinder _settingsBinder;
        private readonly HeroSelectionBinder _heroSelectionBinder;
        
        public HubViewModel(CampaignNavigationBinder campaignNavigationBinder, SettingsBinder settingsBinder, 
            HeroSelectionBinder heroSelectionBinder)
        {
            _campaignNavigationBinder = campaignNavigationBinder;
            _settingsBinder = settingsBinder;
            _heroSelectionBinder = heroSelectionBinder;
        }

        public void OpenCampaign()
        {
            _campaignNavigationBinder.TryBindAndOpen(out _);
            // if (_isMissionsOpened)
            //     return;

            // var campaignViewPrefab = Resources.Load<CampaignNavigationView>(Paths.CAMPAIGN_NAVIGATION_VIEW);
            // var campaignView = UnityEngine.Object.Instantiate(campaignViewPrefab);
            // campaignView.Construct(_audioPlayer);

            // var campaignViewModel = new CampaignNavigationViewModel(_exitToGameplaySignal);
            // campaignViewModel.OnClosingCompleted.Subscribe(_ =>  {
            //     _rootUIBinder.ClearView(campaignView);
            //     _isMissionsOpened = false;
            // });

            // BindCampaignOnLoaded();
            // campaignView.Bind(campaignViewModel);
            // _rootUIBinder.AddView(campaignView);
            // campaignViewModel.Open();
            // _onCampaignOpened.OnNext(campaignViewModel);
            
            // _isMissionsOpened = true;

            // void BindCampaignOnLoaded()
            // {
            //     _campaignModelFactory.GetModelOnLoaded((campaignModel)=>
            //     {
            //         if (campaignViewModel != null && campaignView != null)
            //             campaignViewModel.Bind(campaignModel);
            //     });
            // }
        }

        public void OpenHeroSelection()
        {
            _heroSelectionBinder.TryBindAndOpen(out _);
        }

        public void OpenSettings()
        {
            _settingsBinder.TryBindAndOpen(out _);
        }
    }
}