using System;
using System.Collections.Generic;
using UnityEngine;

using LostKaiju.Game.GameData.Campaign.Locations;

namespace LostKaiju.Game.GameData.Campaign
{
    [Serializable]
    public class CampaignState : IVersioned
    {
        public int Version => _version;
        [SerializeField] private int _version = 1;

        public List<LocationState> Locations;
        public string SelectedLocationId;
        public string SelectedMissionId;
        public string LastLaunchedLocationId;
        public string LastLaunchedMissionId;
    }
}