using System;
using UnityEngine.AddressableAssets;
using R3;

using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.GameData.Campaign
{
    public class CampaignModelFactory : ILoadableModelFactory<CampaignModel>
    {
        public Observable<CampaignModel> OnProduced => _onProduced;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly Subject<CampaignModel> _onProduced = new();

        public CampaignModelFactory(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public void GetModelOnLoaded(Action<CampaignModel> onLoaded)
        {
            var locationsDataSOHandle = Addressables.LoadAssetAsync<AllLocationsDataSO>(Paths.LOCATIONS_DATA);
            locationsDataSOHandle.Completed += (handler) =>
            {
                var locationsDataSO = handler.Result;

                var campaignState = _gameStateProvider.Campaign;
                var campaignModel = new CampaignModel(campaignState, locationsDataSO);

                // check for new campaign data
                if (_gameStateProvider.Campaign.CampaignDataVersion != locationsDataSO.Version)
                {
                    campaignModel.UpdateAvailableLocationsAndMissions();
                    _gameStateProvider.Campaign.CampaignDataVersion = locationsDataSO.Version;
                    _gameStateProvider.SaveCampaignAsync(); // save new version
                }
                onLoaded(campaignModel);
                _onProduced.OnNext(campaignModel);
            };
        }
    }
}