using System.Threading.Tasks;

using LostKaiju.Boilerplates.Locator;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.Providers.GameState
{
    public interface IGameStateProvider : IService
    {
        public SettingsState Settings { get; }
        public CampaignState Campaign { get; }

        public Task LoadCampaignAsync();
        public Task SaveCampaignAsync();
        
        public Task LoadSettingsAsync();
        public Task SaveSettingsAsync();
    }
}
