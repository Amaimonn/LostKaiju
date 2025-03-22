using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Constants;
using LostKaiju.Game.GameData;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class CampaignNavigationBinder : Binder<CampaignNavigationViewModel>
    {
        private readonly ILoadableModelFactory<CampaignModel> _modelFactory;
        private readonly AudioPlayer _audioPlayer;
        private readonly Subject<Unit> _exitToGameplaySignal;
        
        public CampaignNavigationBinder(IRootUIBinder rootUIBinder, ILoadableModelFactory<CampaignModel> factory,
        AudioPlayer audioPlayer, Subject<Unit> exitToGameplaySignal) : base(rootUIBinder)
        {
            _modelFactory = factory;
            _audioPlayer = audioPlayer;
            _exitToGameplaySignal = exitToGameplaySignal;
        }

        public override bool TryBindAndOpen(out CampaignNavigationViewModel viewModel)
        {
            if (_currentViewModel != null) // if already exists
            {
                viewModel = null;
                return false;
            }

            var campaignView = LoadAndInstantiateView<CampaignNavigationView>(Paths.CAMPAIGN_NAVIGATION_VIEW);
            campaignView.Construct(_audioPlayer);

            _currentViewModel = new CampaignNavigationViewModel(_exitToGameplaySignal);
            _currentViewModel.OnClosingCompleted.Subscribe(_ =>  {
                _rootUIBinder.ClearView(campaignView);
            });

            BindCampaignOnLoaded();
            campaignView.OnDisposed.Take(1).Subscribe(_ => {
                // _currentViewModel?.Dispose();
                _currentViewModel = null;
                _modelFactory.Release();
            });
            campaignView.Bind(_currentViewModel);
            _rootUIBinder.AddView(campaignView);
            _currentViewModel.Open();
            _onOpened.OnNext(_currentViewModel);
            
            viewModel = _currentViewModel;
            return true;
            
            void BindCampaignOnLoaded()
            {
                _modelFactory.GetModelOnLoaded(campaignModel =>
                {
                    if (_currentViewModel != null && campaignView != null)
                        _currentViewModel.Bind(campaignModel);
                });
            }
        }
    }
}