using LostKaiju.Game.Constants;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Default;
using LostKaiju.Game.GameData.Settings;
using UnityEngine;

namespace LostKaiju.Game.Providers.DefaultState
{
    public class DefaultStateSOProvider : IDefaultStateProvider
    {
        public CampaignState GetCampaign()
        {
            return Resources.Load<DefaultStateSO>(Paths.DEFAULT_STATE_SO).CampaignState.Copy();
        }

        public SettingsState GetSettings()
        {
            return Resources.Load<DefaultStateSO>(Paths.DEFAULT_STATE_SO).SettingsState.Copy();
        }
    }
}