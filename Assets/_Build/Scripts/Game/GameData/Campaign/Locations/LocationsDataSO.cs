using UnityEngine;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    [CreateAssetMenu(fileName = "AllLocationsDataSO", menuName = "Scriptable Objects/AllLocationsDataSO")]
    public class AllLocationsDataSO : ScriptableObject, IAllLocationsData
    {
        [field: SerializeField] public int Version { get; private set; }
        public ILocationData[] AllData => _locationsData;

        [SerializeField] private LocationDataSO[] _locationsData;
    }
}
