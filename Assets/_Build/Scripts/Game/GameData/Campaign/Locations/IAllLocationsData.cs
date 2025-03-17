namespace LostKaiju.Game.GameData.Campaign.Locations
{
    public interface IAllLocationsData : IVersioned
    {
        public ILocationData[] AllData { get; }
    }
}