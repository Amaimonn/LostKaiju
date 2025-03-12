using System;
using UnityEngine;

using LostKaiju.Game.World.Player.Data.StateParameters;

namespace LostKaiju.Game.World.Player.Data
{
    [Serializable]
    public class PlayerControlsData
    {
        [field: SerializeField] public WalkParameters Walk { get; private set; }
        [field: SerializeField] public JumpParameters Jump { get; private set; }
        [field: SerializeField] public AttackParameters Attack { get; private set; }
    }
}
