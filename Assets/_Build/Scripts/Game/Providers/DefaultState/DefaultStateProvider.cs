using LostKaiju.Game.Constants;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Default;
using LostKaiju.Game.GameData.Heroes;
using LostKaiju.Game.GameData.Settings;
using UnityEngine;

namespace LostKaiju.Game.Providers.DefaultState
{
    public class DefaultStateSOProvider : IDefaultStateProvider
    {
        public CampaignState GetCampaign()
        {
            return Resources.Load<DefaultStateSO>(Paths.DEFAULT_STATE_SO).Campaign.Copy();
        }

        public SettingsState GetSettings()
        {
            return Resources.Load<DefaultStateSO>(Paths.DEFAULT_STATE_SO).Settings.Copy();
        }

        public HeroesState GetHeroes()
        {
            return Resources.Load<DefaultStateSO>(Paths.DEFAULT_STATE_SO).Heroes.Copy();
        }
    }
}