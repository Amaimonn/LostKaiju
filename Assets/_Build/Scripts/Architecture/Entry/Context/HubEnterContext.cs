public class HubEnterContext
{
    public SceneContext FromSceneContext { get; }

    public HubEnterContext(SceneContext fromSceneContext)
    {
        FromSceneContext = fromSceneContext;
    }
}
