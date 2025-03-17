using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using R3;

using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.GameData.Campaign
{
    public class CampaignModelFactory : IAsyncModelFactory<CampaignModel>
    {
        public Observable<CampaignModel> OnProduced => _onProduced;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly Subject<CampaignModel> _onProduced = new();

        public CampaignModelFactory(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public async Task<CampaignModel> GetModelAsync()
        {
            var locationsDataSOHandle = Addressables.LoadAssetAsync<AllLocationsDataSO>(Paths.LOCATIONS_DATA);
            await locationsDataSOHandle.Task;
            var locationsDataSO = locationsDataSOHandle.Result;

            var campaignState = _gameStateProvider.Campaign;
            var campaignModel = new CampaignModel(campaignState, locationsDataSO);

            // check for new campaign data
            if (_gameStateProvider.Campaign.CampaignDataVersion != locationsDataSO.Version)
            {
                campaignModel.UpdateAvailableLocationsAndMissions();
                _gameStateProvider.Campaign.CampaignDataVersion = locationsDataSO.Version;
                await _gameStateProvider.SaveCampaignAsync(); // save new version
            }

            _onProduced.OnNext(campaignModel);

            return campaignModel;
        }
    }
}