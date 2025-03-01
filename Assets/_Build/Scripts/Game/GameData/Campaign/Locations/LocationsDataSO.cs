using System.Collections.Generic;
using UnityEngine;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    [CreateAssetMenu(fileName = "LocationsDataSO", menuName = "Scriptable Objects/LocationsDataSO")]
    public class LocationsDataSO : ScriptableObject, IAllLocationsData
    {
        public ILocationData[] AllData => _locationsData;
        [SerializeField] private LocationDataSO[] _locationsData;
    }
}
