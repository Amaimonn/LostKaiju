using LostKaiju.Configs;
using LostKaiju.Player.View;

namespace LostKaiju.Architecture.Entry.Context
{
    /// <summary>
    /// Info to enter gameplay scene
    /// </summary>
    public class GameplayEnterContext : SceneContext
    {
        public PlayerBinderSO PlayerBinderSO;

        public GameplayEnterContext(string sceneName) : base(sceneName)
        {
        }
    }
}
