using System;
using System.Threading.Tasks;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.Constants;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        public Observable<CampaignNavigationViewModel> OnCampaignOpened => _onCampaignOpened;
        
        private readonly Subject<CampaignNavigationViewModel> _onCampaignOpened = new();
        private readonly Subject<Unit> _exitToGameplaySignal;
        private readonly Func<Task<CampaignModel>> _campaignModelFactory;
        private readonly SettingsBinder _settingsBinder;
        private HeroSelectionBinder _heroSelectionBinder;
        private readonly IRootUIBinder _rootUIBinder;
        private readonly AudioPlayer _audioPlayer;
        private bool _isMissionsOpened = false;
        
        public HubViewModel(Subject<Unit> exitToGameplaySignal, Func<Task<CampaignModel>> campaignModelFactory,
            SettingsBinder settingsBinder, HeroSelectionBinder heroSelectionBinder, IRootUIBinder rootUIBinder,
            AudioPlayer audioPlayer)
        {
            _exitToGameplaySignal = exitToGameplaySignal;
            _campaignModelFactory = campaignModelFactory;
            _settingsBinder = settingsBinder;
            _heroSelectionBinder = heroSelectionBinder;
            _rootUIBinder = rootUIBinder;
            _audioPlayer = audioPlayer;
        }

        public void OpenCampaign()
        {
            if (_isMissionsOpened)
                return;

            var campaignViewPrefab = Resources.Load<CampaignNavigationView>(Paths.CAMPAIGN_NAVIGATION_VIEW);
            var campaignView = UnityEngine.Object.Instantiate(campaignViewPrefab);
            campaignView.Construct(_audioPlayer);

            var campaignViewModel = new CampaignNavigationViewModel(_exitToGameplaySignal);
            campaignViewModel.OnClosingCompleted.Subscribe(_ =>  {
                _rootUIBinder.ClearView(campaignView);
                _isMissionsOpened = false;
            });

            BindCampaignAsync();
            campaignView.Bind(campaignViewModel);
            _rootUIBinder.AddView(campaignView);
            campaignViewModel.Open();
            _onCampaignOpened.OnNext(campaignViewModel);
            
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

        public void OpenHeroSelection()
        {
            _heroSelectionBinder.TryBindAndOpen(out _);
        }

        public void OpenSettings()
        {
            _settingsBinder.ShowSettings();
        }
    }
}