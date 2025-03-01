using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    /// <summary>
    /// Info to enter gameplay scene
    /// </summary>
    public class GameplayEnterContext : SceneContext
    {
        public string LevelSceneName;
        public string PlayerConfigPath;
        public MissionModel SelectedMissionModel;
        public LocationModel SelectedLocationModel;

        public GameplayEnterContext() : base(Scenes.GAMEPLAY)
        {
        }
    }
}
