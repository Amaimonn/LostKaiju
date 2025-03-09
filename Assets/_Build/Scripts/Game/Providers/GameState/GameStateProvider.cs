using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Services.Saves;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.Providers.GameState
{
    public class GameStateProvider : IGameStateProvider
    {
        public SettingsState Settings { get; private set; }
        public CampaignState Campaign { get; private set; }

        private readonly ISaveSystem _saveSystem;

        public GameStateProvider(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public async Task LoadCampaignAsync()
        {
            bool exists = await _saveSystem.ExistsAsync(StateKeys.CAMPAIGN);
            if (exists)
                Campaign = await _saveSystem.LoadAsync<CampaignState>(StateKeys.CAMPAIGN);
            else
                InitializeAndSaveCampaign();
        }

        public async Task SaveCampaignAsync()
        {
            await _saveSystem.SaveAsync(StateKeys.CAMPAIGN, Campaign);
        }

        public async Task LoadSettingsAsync()
        {
            bool exists = await _saveSystem.ExistsAsync(StateKeys.SETTINGS);
            if (exists)
                Settings = MigrateSettings(await _saveSystem.LoadAsync<SettingsState>(StateKeys.SETTINGS));
            else
                InitializeAndSaveSettings();
        }

        public async Task SaveSettingsAsync()
        {
            await _saveSystem.SaveAsync(StateKeys.SETTINGS, Settings);
        }

        private void InitializeAndSaveCampaign()
        {
            var missions = new List<MissionState>()
            {
                new (id:"1_1", isCompleted: false)
            };

            var location = new LocationState(id: "1", isCompleted: false, openedMissions: missions);

            Campaign = new CampaignState()
            {
                Locations = new List<LocationState> { location }
            };

            Debug.Log("Campaign load: init");
            _saveSystem.SaveAsync(StateKeys.CAMPAIGN, Campaign);
        }

        private void InitializeAndSaveSettings()
        {
            Settings = new SettingsState()
            {
                SoundVolume = 60,
                IsSoundEnabled = true,
                SfxVolume = 60,
                IsSfxEnabled = true,
                Brightness = 80,
                IsPostProcessingEnabled = true,
                IsHighBloomQuality = false,
                IsAntiAliasingEnabled = false
            };

            Debug.Log("Settings load: init");
            _saveSystem.SaveAsync(StateKeys.SETTINGS, Settings);
        }
        
        private SettingsState MigrateSettings(SettingsState settingsState)
        {
            if (settingsState.Version != 1)
            {
                settingsState.Brightness = 80;
                settingsState.IsHighBloomQuality = false;
                settingsState.IsAntiAliasingEnabled = false;
                _saveSystem.SaveAsync(StateKeys.SETTINGS, Settings); 
            }
            
            return settingsState;
        }   
    }
}
