using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public abstract class MissionBootstrap : LifetimeScope
    {
        public abstract Observable<MissionExitContext> Boot(MissionEnterContext missionEnterContext);
    }
}