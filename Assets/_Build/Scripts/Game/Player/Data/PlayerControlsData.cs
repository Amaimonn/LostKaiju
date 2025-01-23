using System;
using UnityEngine;

using LostKaiju.Game.Player.Data.StateParameters;

namespace LostKaiju.Game.Player.Data
{
    [Serializable]
    public class PlayerControlsData
    {
        [field: SerializeField] public WalkParameters Walk { get; private set; }
        [field: SerializeField] public JumpParameters Jump { get; private set; }
    }
}
