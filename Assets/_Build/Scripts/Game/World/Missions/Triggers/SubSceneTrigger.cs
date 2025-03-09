using LostKaiju.Game.World.Player.Views;

namespace LostKaiju.Game.World.Missions.Triggers
{
    public class SubSceneTrigger : TargetTrigger<IPlayerHero>
    {
        public string ToSceneName;
    }
}