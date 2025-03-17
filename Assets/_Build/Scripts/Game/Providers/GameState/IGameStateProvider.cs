using System.Threading.Tasks;

using LostKaiju.Boilerplates.Locator;
using LostKaiju.Game.GameData.Default;
namespace LostKaiju.Game.Providers.GameState
{
    public interface IGameStateProvider : IDefaultState, IService
    {
        public Task LoadSettingsAsync();
        public Task SaveSettingsAsync();

        public Task LoadCampaignAsync();
        public Task SaveCampaignAsync();
        
        public Task LoadHeroesAsync();
        public Task SaveHeroesAsync();
    }
}
