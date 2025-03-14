using System;
using UnityEngine;

using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    [Serializable]
    public class LocationData : ILocationData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        public IMissionData[] AllMissionsData => AllMissionsDataRaw;
        
        [field: SerializeField] public MissionData[] AllMissionsDataRaw { get; private set; }

    }
}