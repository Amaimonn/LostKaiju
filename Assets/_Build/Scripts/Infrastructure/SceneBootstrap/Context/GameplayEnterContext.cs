using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;
using R3;

namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    /// <summary>
    /// Info to enter gameplay scene
    /// </summary>
    public class GameplayEnterContext : SceneContext
    {
        public string LevelSceneName;
        public string PlayerConfigPath;
        public IMissionData SelectedMissionData;
        public ILocationData SelectedLocationData;
        public Subject<Unit> MissionCompletionSignal;

        public GameplayEnterContext() : base(Scenes.GAMEPLAY)
        {
        }
    }
}
