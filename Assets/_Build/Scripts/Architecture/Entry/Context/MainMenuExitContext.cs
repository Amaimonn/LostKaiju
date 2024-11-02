/// <summary>
/// Info to enter the Hub scene from the Main menu. Exiting the Main menu leads to the Hub.
/// </summary>
public class MainMenuExitContext
{
    public HubEnterContext HubEnterContext { get; }

    public MainMenuExitContext(HubEnterContext hubEnterContext)
    {
        HubEnterContext = hubEnterContext;
    }
}
