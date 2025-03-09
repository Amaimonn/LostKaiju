using System;
using System.Collections.Generic;
using UnityEngine;

using LostKaiju.Game.GameData.Campaign.Locations;
using System.Linq;

namespace LostKaiju.Game.GameData.Campaign
{
    [Serializable]
    public class CampaignState : IVersioned, ICopyable<CampaignState>
    {
        public int Version { get => _version; set => _version = value; }
        [SerializeField] private int _version = 1;

        public int LastUpdateIndex = 1; // to update the saved state when new missions are added
        public List<LocationState> Locations;
        public string SelectedLocationId;
        public string SelectedMissionId;
        public string LastLaunchedLocationId;
        public string LastLaunchedMissionId;

        public CampaignState Copy()
        {
            var copiedLocations = Locations?.Select(x => x.Copy()).ToList();
            var copy = (CampaignState)MemberwiseClone();
            copy.Locations = copiedLocations;
            
            return copy;
        }
    }
}