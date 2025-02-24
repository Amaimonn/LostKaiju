using System;
using System.Threading.Tasks;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Campaign;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        private readonly Subject<Unit> _exitSubject;
        private readonly Func<Task<CampaignModel>> _campaignModelFactory;
        private readonly IRootUIBinder _rootUIBinder;
        private bool _isMissionsOpened = false;
        private bool _isHeroSelectionOpened = false;
        private const string CAMPAIGN_NAVIGATION_VIEW_PATH = "UI/Hub/CampaignNavigationView";
        private const string HERO_SELECTION_VIEW_PATH = "UI/Hub/HeroSelectionView";
        
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

        public void OpenHeroSelection()
        {
            if (_isHeroSelectionOpened)
                return;
            
            var heroSelectionViewPrefab = Resources.Load<HeroSelectionView>(HERO_SELECTION_VIEW_PATH);
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
    }
}