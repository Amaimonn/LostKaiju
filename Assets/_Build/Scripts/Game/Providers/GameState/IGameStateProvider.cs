using LostKaiju.Game.GameData;
using LostKaiju.Game.GameData.Campaign;

namespace LostKaiju.Game.Providers.GameState
{
    public interface IGameStateProvider
    {
        public SettingsModel Settings { get; }
        public CampaignModel Missions { get; }
    }
}
