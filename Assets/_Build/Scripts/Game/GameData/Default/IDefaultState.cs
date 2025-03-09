using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.GameData.Default
{
    public interface IDefaultState
    {
        public SettingsState SettingsState { get; }
        public CampaignState CampaignState { get; }
    }
}