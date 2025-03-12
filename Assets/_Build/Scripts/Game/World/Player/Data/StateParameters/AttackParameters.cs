using System;
using UnityEngine;

namespace LostKaiju.Game.World.Player.Data.StateParameters
{
    [Serializable]
    public class AttackParameters
    {
        [field: SerializeField, Min(0)] public float Cooldown { get; private set; } = 0.2f;
    }
}
