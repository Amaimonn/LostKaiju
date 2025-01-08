namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    /// <summary>
    /// Info to get back to the Hub from the Gameplay scene. Exiting the Gameplay leads to the Hub.
    /// </summary>
    public class GameplayExitContext
    {
        public HubEnterContext HubEnterContext { get; }

        public GameplayExitContext(HubEnterContext hubEnterContext)
        {
            HubEnterContext = hubEnterContext;
        }
    }
}
