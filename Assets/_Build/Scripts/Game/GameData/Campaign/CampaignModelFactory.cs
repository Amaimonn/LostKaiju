using System.Threading.Tasks;
using UnityEngine;
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
            var locationsDataSORequest = Resources.LoadAsync<LocationsDataSO>(Paths.LOCATIONS_DATA);
            await locationsDataSORequest;

            var locationsDataSO = locationsDataSORequest.asset as LocationsDataSO;
            var campaignState = _gameStateProvider.Campaign;
            var campaignModel = new CampaignModel(campaignState, locationsDataSO);

            _onProduced.OnNext(campaignModel);

            return campaignModel;
        }
    }
}