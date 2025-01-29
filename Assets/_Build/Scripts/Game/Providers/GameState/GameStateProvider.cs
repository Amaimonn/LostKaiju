using System.Collections.Generic;
using System.Threading.Tasks;

using LostKaiju.Game.GameData;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Services.Saves;


namespace LostKaiju.Game.Providers.GameState
{
    public class GameStateProvider : IGameStateProvider
    {
        public SettingsState Settings { get; private set; }
        public CampaignState Campaign { get; private set; }

        private readonly ISaveSystem _saveSystem;
        private const string CAMPAIGN_STATE_KEY = "CampaignState";

        public GameStateProvider(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public async Task LoadCampaignStateAsync()
        {
            bool exists = await _saveSystem.ExistsAsync(CAMPAIGN_STATE_KEY);
            if (exists)
            {
                Campaign = await _saveSystem.LoadAsync<CampaignState>(CAMPAIGN_STATE_KEY);
            }
            else
            {
                // simple initial state from settings
                var missions = new List<MissionState>()
                {
                    new (id:"1_1", isOpened: true, isCompleted: false)
                };

                var location = new LocationState(id: "1", isOpened: true, openedMissions: missions);

                Campaign = new CampaignState()
                { 
                    Locations = new List<LocationState>{ location }
                };
                await _saveSystem.SaveAsync(CAMPAIGN_STATE_KEY, Campaign);
            }
        }
    }
}
