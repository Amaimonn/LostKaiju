using UnityEngine;
using YG;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.DefaultState;
using LostKaiju.Game.GameData.Heroes;
using LostKaiju.Utils;

namespace LostKaiju.Game.Providers.GameState
{
    public class GameStateProviderYG : IGameStateProvider
    {
        public SettingsState Settings { get => YG2.saves.Settings; private set => YG2.saves.Settings = value; }
        public CampaignState Campaign { get => YG2.saves.Campaign; private set => YG2.saves.Campaign = value; }
        public HeroesState Heroes { get => YG2.saves.Heroes; private set => YG2.saves.Heroes = value; }
        
        private readonly IDefaultStateProvider _defaultStateProvider;

        public GameStateProviderYG(IDefaultStateProvider defaultStateProvider)
        {
            _defaultStateProvider = defaultStateProvider;
        }

        public void LoadCampaign()
        {
            if (YG2.saves.Campaign == null)
                InitializeAndSaveCampaign();
        }

        public void LoadSettings()
        {
            if (YG2.saves.Settings == null)
                InitializeAndSaveSettings();

            if (!YG2.saves.Settings.IsLanguageSelected)
                Settings.LanguageIndex = LocaleHelper.GetLanguageIndexByCode(YG2.lang);
        }

        public void LoadHeroes()
        {
            if (YG2.saves.Heroes == null)
                InitializeAndSaveHeroes();
        }

        public void SaveCampaign()
        {
            YG2.SaveProgress();
        }

        public void SaveSettings()
        {
            YG2.SaveProgress();
        }

        public void SaveHeroes()
        {
            YG2.SaveProgress();
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

        private void InitializeAndSaveHeroes()
        {
            Heroes = _defaultStateProvider.GetHeroes();
            Debug.Log("Heroes load: init");
            YG2.SaveProgress();
        }
    }
}