using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    public interface ILocationData
    {
        public string Id { get; }
        public IMissionData[] AllMissionsData { get; }
    }
}