using System;
using UnityEngine;

namespace LostKaiju.Gameplay.Player.Data.StateParameters
{
    [Serializable]
    public class JumpParameters
    {
        [field: SerializeField, Min(0)] public float Force { get; private set; }
        [field: SerializeField, Min(0)] public float CoyotteTime { get; private set; }
        [field: SerializeField, Min(0)] public float Cooldown  { get; private set; }
        [field: SerializeField, Min(0)] public float InputTimeBufferSize  { get; private set; }
    }
}