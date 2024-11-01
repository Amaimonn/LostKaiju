/// <summary>
/// Info to enter target scene from main menu
/// </summary>
public class MainMenuExitContext
{
    public SceneContext TagretSceneContext { get; }

    public MainMenuExitContext(SceneContext tagretSceneContext)
    {
        TagretSceneContext = tagretSceneContext;
    }
}
