using UnityEngine;

namespace LostKaiju.Infrastructure.SceneBootstrap.Context
{
    /// <summary>
    /// Info to enter Mission scene (from Hub or additive mission scene)
    /// </summary>
    public class MissionEnterContext
    {
        // Mission scope context. Primarily, some data for transition between mission scenes
        public string FromMissionSceneName;
        public Vector3? PlayerPosition = null;
        public string FromTriggerId;
        
        // gameplay scope context
        public GameplayEnterContext GameplayEnterContext { get; }

        public MissionEnterContext(GameplayEnterContext gameplayEnterContext)
        {
            GameplayEnterContext = gameplayEnterContext;
        }
    }
}