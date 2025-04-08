using LostKaiju.Boilerplates.Locator;
using LostKaiju.Game.GameData.Default;

namespace LostKaiju.Game.Providers.GameState
{
    public interface IGameStateProvider : IDefaultState, IService
    {
        public void LoadSettings();
        public void SaveSettings();

        public void LoadCampaign();
        public void SaveCampaign();
        
        public void LoadHeroes();
        public void SaveHeroes();
    }
}
