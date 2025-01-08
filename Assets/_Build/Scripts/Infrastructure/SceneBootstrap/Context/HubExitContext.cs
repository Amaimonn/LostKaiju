namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    public class HubExitContext
    {
        public SceneContext ToSceneContext { get; }

        public HubExitContext(SceneContext toSceneContext)
        {
            ToSceneContext = toSceneContext;
        }
    }
}
