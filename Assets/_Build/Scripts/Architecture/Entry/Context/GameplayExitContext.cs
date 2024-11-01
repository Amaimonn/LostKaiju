/// <summary>
/// Info to get back to hub from gameplay scene
/// </summary>
public class GameplayExitContext
{
    public HubEnterContext HubEnterContext { get; }

    public GameplayExitContext(HubEnterContext hubEnterContext)
    {
        HubEnterContext = hubEnterContext;
    }
}
