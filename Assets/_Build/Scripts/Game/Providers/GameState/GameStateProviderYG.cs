using System.Threading.Tasks;
using UnityEngine;
using YG;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.DefaultState;

namespace LostKaiju.Game.Providers.GameState
{
    public class GameStateProviderYG : IGameStateProvider
    {
        public SettingsState Settings { get => YG2.saves.Settings; private set => YG2.saves.Settings = value; }
        public CampaignState Campaign { get => YG2.saves.Campaign; private set => YG2.saves.Campaign = value; }
        
        private readonly IDefaultStateProvider _defaultStateProvider;

        public GameStateProviderYG(IDefaultStateProvider defaultStateProvider)
        {
            _defaultStateProvider = defaultStateProvider;
        }

        public Task LoadCampaignAsync()
        {
            if (YG2.saves.Campaign == null)
                InitializeAndSaveCampaign();
            return Task.CompletedTask;
        }

        public Task LoadSettingsAsync()
        {
            if (YG2.saves.Settings == null)
                InitializeAndSaveSettings();
            return Task.CompletedTask;
        }

        public Task SaveCampaignAsync()
        {
            YG2.SaveProgress();
            return Task.CompletedTask;
        }

        public Task SaveSettingsAsync()
        {
            YG2.SaveProgress();
            return Task.CompletedTask;
        }

        private void InitializeAndSaveCampaign()
        {
            Campaign = _defaultStateProvider.GetCampaign();
            Debug.Log("Campaign load: init");
            YG2.SaveProgress();
        }

        private void InitializeAndSaveSettings()
        {
            Settings = _defaultStateProvider.GetSettings();
            Debug.Log("Settings load: init");
            YG2.SaveProgress();
        }
    }
}