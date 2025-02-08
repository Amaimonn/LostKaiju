namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    /// <summary>
    /// Info to enter Mission scene (from Hub or additive mission scene)
    /// </summary>
    public class MissionEnterContext
    {
        // some mission data to pass to the mission scene
        public string MissionName;
        public GameplayEnterContext GameplayEnterContext { get; }

        public MissionEnterContext(GameplayEnterContext gameplayEnterContext)
        {
            GameplayEnterContext = gameplayEnterContext;
        }
    }
}