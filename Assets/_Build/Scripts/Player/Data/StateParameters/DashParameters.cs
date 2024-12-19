using System;
using UnityEngine;

namespace LostKaiju.Player.Data.StateParameters
{
    [Serializable]
    public class DashParameters
    {
        [field: SerializeField, Min(0)] public float Distance { get; private set; } = 7f;
        [field: SerializeField, Min(0)] public float Duration { get; private set; } = 0.4f;
        [field: SerializeField, Min(0)] public float Cooldown { get; private set; } = 2f;
        [field: SerializeField] public float CollisionLayerMask { get; private set; }

        [HideInInspector]
        public Rigidbody2D rigidBody;
    }
}
