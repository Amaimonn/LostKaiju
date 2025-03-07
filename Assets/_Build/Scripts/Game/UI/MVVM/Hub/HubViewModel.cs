using System;
using System.Threading.Tasks;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.UI.MVVM.Shared.SettingsDyn;
using LostKaiju.Game.UI.Constants;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        public Observable<CampaignNavigationViewModel> OnCampaignOpened => _onCampaignOpened;
        private readonly Subject<CampaignNavigationViewModel> _onCampaignOpened;
        private readonly Subject<Unit> _exitToGameplaySignal;
        private readonly Func<Task<CampaignModel>> _campaignModelFactory;
        private readonly SettingsBinder _settingsBinder;
        private readonly IRootUIBinder _rootUIBinder;
        private bool _isMissionsOpened = false;
        private bool _isHeroSelectionOpened = false;
        
        public HubViewModel(Subject<Unit> exitToGameplaySignal, Func<Task<CampaignModel>> campaignModelFactory,
            SettingsBinder settingsBinder, IRootUIBinder rootUIBinder)
        {
            _exitToGameplaySignal = exitToGameplaySignal;
            _campaignModelFactory = campaignModelFactory;
            _settingsBinder = settingsBinder;
            _rootUIBinder = rootUIBinder;
        }

        public void OpenCampaign()
        {
            if (_isMissionsOpened)
                return;

            var campaignViewPrefab = Resources.Load<CampaignNavigationView>(Paths.CAMPAIGN_NAVIGATION_VIEW_PATH);
            var campaignView = UnityEngine.Object.Instantiate(campaignViewPrefab);

            var campaignViewModel = new CampaignNavigationViewModel(_exitToGameplaySignal);
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

        public void OpenHeroSelection()
        {
            if (_isHeroSelectionOpened)
                return;
            
            var heroSelectionViewPrefab = Resources.Load<HeroSelectionView>(Paths.HERO_SELECTION_VIEW_PATH);
            var heroSelectionView = UnityEngine.Object.Instantiate(heroSelectionViewPrefab);

            var heroSelectionViewModel = new HeroSelectionViewModel();
            heroSelectionViewModel.OnClosingCompleted.Subscribe(_ =>  {
                _rootUIBinder.ClearView(heroSelectionView);
                _isHeroSelectionOpened = false;
            });

            heroSelectionView.Bind(heroSelectionViewModel);
            _rootUIBinder.AddView(heroSelectionView);
            heroSelectionViewModel.Open();

            _isHeroSelectionOpened = true;
        }

        public void OpenSettings()
        {
            _settingsBinder.ShowSettings();
        }
    }
}