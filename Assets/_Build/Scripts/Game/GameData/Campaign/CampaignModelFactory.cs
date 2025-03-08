using System.Threading.Tasks;
using UnityEngine;
using R3;

using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Campaign.Locations;

namespace LostKaiju.Game.GameData.Campaign
{
    public class CampaignModelFactory
    {
        public Subject<CampaignModel> OnProduced { get; } = new Subject<CampaignModel>();

        public async Task<CampaignModel> GetModelAsync(IGameStateProvider gameStateProvider)
        {
            var locationsDataSORequest = Resources.LoadAsync<LocationsDataSO>(Paths.LOCATIONS_DATA_PATH);
            await locationsDataSORequest;

            var locationsDataSO = locationsDataSORequest.asset as LocationsDataSO;
            var campaignState = gameStateProvider.Campaign;
            var campaignModel = new CampaignModel(campaignState, locationsDataSO);

            OnProduced.OnNext(campaignModel);

            return campaignModel;
        }
    }
}