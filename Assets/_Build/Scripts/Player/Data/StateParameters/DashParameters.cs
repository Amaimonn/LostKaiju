using System;
using UnityEngine;

namespace Assets._Build.Scripts.Player.Data.StateParameters
{
    [Serializable]
    public class DashParameters
    {
        public float Distance => _distance;
        public float Duration => _duration;
        public float Cooldown => _cooldown;
        public float CollisionLayerMask => _collisionLayerMask;

        [SerializeField] private float _distance = 7f;
        [SerializeField] private float _duration = 0.4f;
        [SerializeField] private float _cooldown = 2f;
        [SerializeField] private LayerMask _collisionLayerMask;

        
        [HideInInspector]
        public Rigidbody2D rigidBody;
    }
}
