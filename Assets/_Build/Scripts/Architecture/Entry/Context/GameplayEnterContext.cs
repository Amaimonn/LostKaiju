using LostKaiju.Configs;

namespace LostKaiju.Architecture.Entry.Context
{
    /// <summary>
    /// Info to enter gameplay scene
    /// </summary>
    public class GameplayEnterContext : SceneContext
    {
        public string LevelSceneName;
        public ICreatureConfig PlayerConfig;

        public GameplayEnterContext(string sceneName) : base(sceneName)
        {
        }
    }
}
