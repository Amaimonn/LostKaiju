using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Heroes;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.Providers.DefaultState
{
    public interface IDefaultStateProvider
    {
        public SettingsState GetSettings();
        public CampaignState GetCampaign();
        public HeroesState GetHeroes();
    }
}