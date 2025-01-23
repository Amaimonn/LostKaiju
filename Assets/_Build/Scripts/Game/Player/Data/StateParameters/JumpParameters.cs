using System;
using UnityEngine;

namespace LostKaiju.Game.Player.Data.StateParameters
{
    [Serializable]
    public class JumpParameters
    {
        [field: SerializeField, Min(0)] public float Force { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float CoyotteTime { get; private set; } = 0.1f;
        [field: SerializeField, Min(0)] public float Cooldown  { get; private set; } = 0.2f;
        [field: SerializeField, Min(0)] public float InputTimeBufferSize  { get; private set; } = 0.1f;
    }
}