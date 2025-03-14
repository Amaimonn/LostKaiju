using LostKaiju.Game.GameData.Campaign.Missions;
using UnityEngine;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    [CreateAssetMenu(fileName = "LocationDataSO", menuName = "Scriptable Objects/LocationDataSO")]
    public class LocationDataSO : ScriptableObject, ILocationData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        public IMissionData[] AllMissionsData => AllMissionsDataSO;

        [field: SerializeField] public MissionDataSO[] AllMissionsDataSO {get; private set;}
    }
}
