using LostKaiju.Boilerplates.Locator;
using LostKaiju.Game.GameData;
using LostKaiju.Game.GameData.Campaign;

namespace LostKaiju.Game.Providers.GameState
{
    public interface IGameStateProvider : IService
    {
        public SettingsState Settings { get; }
        public CampaignState Campaign { get; }
    }
}
