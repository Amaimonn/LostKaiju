/// <summary>
/// Info to enter target scene from main menu
/// </summary>
public class MainMenuExitContext : SceneContext
{
    public SceneContext TagretSceneContext { get; }

    public MainMenuExitContext(string sceneName) : base(sceneName)
    {
    }
}