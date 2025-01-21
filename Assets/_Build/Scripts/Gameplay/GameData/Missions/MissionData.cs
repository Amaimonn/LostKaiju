using System;
using UnityEngine;

namespace LostKaiju.Game.GameData.Missions
{
    [Serializable]
    public class MissionData
    {
        [field: SerializeField] public string Id { get; private set; }

        /// <summary>
        /// The number of the mission that is displayed on the screen.
        /// </summary>
        [field: SerializeField] public string DysplayedNumber { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public string SceneName { get; private set; }
    }
}