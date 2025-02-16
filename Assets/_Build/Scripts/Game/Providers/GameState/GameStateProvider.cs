using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Services.Saves;


namespace LostKaiju.Game.Providers.GameState
{
    public class GameStateProvider : IGameStateProvider
    {
        public SettingsState Settings { get; private set; }
        public CampaignState Campaign { get; private set; }

        private readonly ISaveSystem _saveSystem;
        private const string CAMPAIGN_STATE_KEY = "CampaignState";
        private const string SETTINGS_STATE_KEY = "SettingsState";

        public GameStateProvider(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public async Task LoadCampaignAsync()
        {
            bool exists = await _saveSystem.ExistsAsync(CAMPAIGN_STATE_KEY);
            if (exists)
                Campaign = await _saveSystem.LoadAsync<CampaignState>(CAMPAIGN_STATE_KEY);
            else
                InitializeAndSaveCampaign();
        }

        public async Task SaveCampaignAsync()
        {
            await _saveSystem.SaveAsync(CAMPAIGN_STATE_KEY, Campaign);
        }

        public async Task LoadSettingsAsync()
        {
            bool exists = await _saveSystem.ExistsAsync(SETTINGS_STATE_KEY);
            if (exists)
                Settings = await _saveSystem.LoadAsync<SettingsState>(SETTINGS_STATE_KEY);
            else
                InitializeAndSaveSettings();
        }

        public async Task SaveSettingsAsync()
        {
            await _saveSystem.SaveAsync(SETTINGS_STATE_KEY, Settings);
        }

        private void InitializeAndSaveCampaign()
        {
            // simple initial state from settings
            var missions = new List<MissionState>()
            {
                new (id:"1_1", isOpened: true, isCompleted: false)
            };

            var location = new LocationState(id: "1", isOpened: true, openedMissions: missions);

            Campaign = new CampaignState()
            {
                Locations = new List<LocationState> { location }
            };

            Debug.Log("Campaign load: init");
            _saveSystem.SaveAsync(CAMPAIGN_STATE_KEY, Campaign);
        }

        private void InitializeAndSaveSettings()
        {
            Settings = new SettingsState()
            {
                SoundVolume = 60,
                IsSoundEnabled = true,
                SfxVolume = 60,
                IsSfxEnabled = true,
                Brightness = 80
            };

            Debug.Log("Settings load: init");
            _saveSystem.SaveAsync(SETTINGS_STATE_KEY, Settings);
        }
    }
}
