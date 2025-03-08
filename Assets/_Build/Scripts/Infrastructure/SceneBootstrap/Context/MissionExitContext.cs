namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    /// <summary>
    /// Info to get from one Mission scene to another (additive) scene of the current mission.
    /// </summary>
    public class MissionExitContext
    {
        public string ToMissionSceneName;
        public MissionEnterContext MissionEnterContext { get; }

        public MissionExitContext(MissionEnterContext missionEnterContext)
        {
            MissionEnterContext = missionEnterContext;
        }
    }
}
