using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Heroes;

namespace LostKaiju.Game.GameData.Default
{
    public interface IDefaultState
    {
        public SettingsState Settings { get; }
        public CampaignState Campaign { get; }
        public HeroesState Heroes { get; }
    }
}