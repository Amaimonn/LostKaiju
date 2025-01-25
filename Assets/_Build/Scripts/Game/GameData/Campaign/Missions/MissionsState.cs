using ObservableCollections;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    /// <summary>
    /// Used with save system/load functions to store opened missions state
    /// </summary>
    public class MissionsState
    {
        public readonly ObservableDictionary<string, ObservableList<string>> LocationsOpenedMissionsMap;
    }
}