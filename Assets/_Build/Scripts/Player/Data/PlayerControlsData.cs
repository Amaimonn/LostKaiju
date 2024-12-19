using System;
using UnityEngine;

using LostKaiju.Player.Data.StateParameters;

namespace LostKaiju.Player.Data
{
    [Serializable]
    public class PlayerControlsData
    {
        public WalkParameters Walk => _walk;
        public JumpParameters Jump => _jump;

        [SerializeField] private WalkParameters _walk;
        [SerializeField] private JumpParameters _jump;
    }
}
