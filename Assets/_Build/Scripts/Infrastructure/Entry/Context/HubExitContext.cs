namespace LostKaiju.Infrastructure.Entry.Context
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
