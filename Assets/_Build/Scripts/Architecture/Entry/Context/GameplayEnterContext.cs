using LostKaiju.Configs;
using LostKaiju.Player.View;

namespace LostKaiju.Architecture.Entry.Context
{
    /// <summary>
    /// Info to enter gameplay scene
    /// </summary>
    public class GameplayEnterContext : SceneContext
    {
        public PlayerBinder PlayerBinder;

        public GameplayEnterContext(string sceneName) : base(sceneName)
        {
        }
    }
}
