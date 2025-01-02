using System;
using UnityEngine;

namespace LostKaiju.Gameplay.Player.Data
{
    [Serializable]
    public class PlayerDefenceData
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public int Defence { get; private set; }
    }
}
