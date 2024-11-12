using System;
using UnityEngine;

namespace LostKaiju.Player.Data.StateParameters
{
    [Serializable]
    public class WalkParameters
    {

        public float WalkSpeed => _walkSpeed;
        public float FrictionKoefficient => _frictionKoefficient;
        public float Acceleration => _acceleration;
        public float Deceleration => _deceleration;
        public float AirMultiplier => _airMultiplier;
        [HideInInspector] public Rigidbody2D WalkRigidbody;

        [SerializeField] private float _walkSpeed = 10;
        [SerializeField] private float _frictionKoefficient = 0.4f;
        [SerializeField] private float _acceleration = 7;
        [SerializeField] private float _deceleration = 7;
        [SerializeField] private float _airMultiplier = 0.5f;
    }
}
