using System;
using UnityEngine;

namespace LostKaiju.Gameplay.Player.Data.StateParameters
{
    [Serializable]
    public class WalkParameters
    {
        [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 10;
        [field: SerializeField, Range(0.0f, 1.0f)] public float FrictionForce { get; private set; } = 0.4f;
        [field: SerializeField, Min(0)] public float Acceleration { get; private set; } = 7;
        [field: SerializeField, Min(0)] public float Deceleration { get; private set; } = 7;
        [field: SerializeField, Min(0)] public float AirMultiplier { get; private set; } = 0.5f;
    }
}