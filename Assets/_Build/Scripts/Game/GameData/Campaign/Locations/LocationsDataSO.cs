using System.Collections.Generic;
using UnityEngine;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    [CreateAssetMenu(fileName = "LocationsDataSO", menuName = "Scriptable Objects/LocationsDataSO")]
    public class LocationsDataSO : ScriptableObject
    {
        [field: SerializeField] public List<LocationDataSO> Locations { get; private set; }
    }
}
