using System;
using UnityEngine;

namespace LostKaiju.Game.World.Player.Data.StateParameters
{
    [Serializable]
    public class DashParameters
    {
        [field: SerializeField, Min(0)] public float Distance { get; private set; } = 7f;
        [field: SerializeField, Min(0)] public float Duration { get; private set; } = 0.4f;
        [field: SerializeField, Min(0)] public float Cooldown { get; private set; } = 0.6f;
        [field: SerializeField] public float CollisionLayerMask { get; private set; }
    }
}
