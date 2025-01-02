using System;
using UnityEngine;

using LostKaiju.Gameplay.Player.Data.StateParameters;

namespace LostKaiju.Gameplay.Player.Data
{
    [Serializable]
    public class PlayerControlsData
    {
        [field: SerializeField] public WalkParameters Walk { get; private set; }
        [field: SerializeField] public JumpParameters Jump { get; private set; }
    }
}
