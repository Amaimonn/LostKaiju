namespace Assets._Build.Scripts.Architecture.Entry.Context
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
