namespace LostKaiju.Infrastructure.Entry.Context
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
