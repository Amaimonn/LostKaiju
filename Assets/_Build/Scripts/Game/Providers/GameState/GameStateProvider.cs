using UnityEngine;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Services.Saves;
using LostKaiju.Game.Constants;
using LostKaiju.Game.Providers.DefaultState;
using LostKaiju.Game.GameData.Heroes;

namespace LostKaiju.Game.Providers.GameState
{
    public class GameStateProvider : IGameStateProvider
    {
        public SettingsState Settings { get; private set; }
        public CampaignState Campaign { get; private set; }
        public HeroesState Heroes { get; private set; }

        private readonly ISaveSystem _saveSystem;
        private readonly IDefaultStateProvider _defaultStateProvider;

        public GameStateProvider(ISaveSystem saveSystem, IDefaultStateProvider defaultStateProvider)
        {
            _saveSystem = saveSystem;
            _defaultStateProvider = defaultStateProvider;
        }

        public void LoadCampaign()
        {
            bool exists = _saveSystem.Exists(StateKeys.CAMPAIGN);
            if (exists)
                Campaign = _saveSystem.Load<CampaignState>(StateKeys.CAMPAIGN);
            else
                InitializeAndSaveCampaign();
        }

        public void SaveCampaign()
        {
            _saveSystem.Save(StateKeys.CAMPAIGN, Campaign);
        }

        public void LoadSettings()
        {
            bool exists = _saveSystem.Exists(StateKeys.SETTINGS);
            if (exists)
                Settings = MigrateSettings(_saveSystem.Load<SettingsState>(StateKeys.SETTINGS));
            else
                InitializeAndSaveSettings();
        }

        public void SaveSettings()
        {
            _saveSystem.Save(StateKeys.SETTINGS, Settings);
        }

        public void LoadHeroes()
        {
            bool exists = _saveSystem.Exists(StateKeys.HEROES);
            if (exists)
                Heroes = _saveSystem.Load<HeroesState>(StateKeys.HEROES);
            else
                InitializeAndSaveHeroes();
        }

        public void SaveHeroes()
        {
            _saveSystem.Save(StateKeys.HEROES, Heroes);
        }

        private void InitializeAndSaveCampaign()
        {
            Campaign = _defaultStateProvider.GetCampaign();

            Debug.Log("Campaign load: init");
            _saveSystem.Save(StateKeys.CAMPAIGN, Campaign);
        }

        private void InitializeAndSaveSettings()
        {
            Settings = _defaultStateProvider.GetSettings();

            Debug.Log("Settings load: init");
            _saveSystem.Save(StateKeys.SETTINGS, Settings);
        }

        private void InitializeAndSaveHeroes()
        {
            Heroes = _defaultStateProvider.GetHeroes();

            Debug.Log("Heroes load: init");
            _saveSystem.Save(StateKeys.HEROES, Heroes);
        }
        
        private SettingsState MigrateSettings(SettingsState settingsState)
        {
            // if (settingsState.Version == 2)
            // {
            //     settingsState.Brightness = 80;
            //     settingsState.IsBloomEnabled = false;
            //     settingsState.IsAntiAliasingEnabled = false;
            //     _saveSystem.SaveAsync(StateKeys.SETTINGS, Settings); 
            // }
            
            return settingsState;
        }   
    }
}
