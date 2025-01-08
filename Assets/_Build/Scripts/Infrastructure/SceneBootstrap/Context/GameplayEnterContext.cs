namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    /// <summary>
    /// Info to enter gameplay scene
    /// </summary>
    public class GameplayEnterContext : SceneContext
    {
        public string LevelSceneName;
        public string PlayerConfigPath;

        public GameplayEnterContext(string sceneName) : base(sceneName)
        {
        }
    }
}
