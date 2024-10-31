/// <summary>
/// Info to get back to main menu from gameplay scene
/// </summary>
public class GameplayExitContext
{
    public MainMenuEnterContext MainMenuEnterContext { get; }

    public GameplayExitContext(MainMenuEnterContext mainMenuEnterContext)
    {
        MainMenuEnterContext = mainMenuEnterContext;
    }
}
