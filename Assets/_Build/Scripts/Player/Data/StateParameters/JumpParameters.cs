using System;
using UnityEngine;

namespace LostKaiju.Player.Data.StateParameters
{
    [Serializable]
    public class JumpParameters
    {
        public float JumpForce => _jumpForce;
        public float JumpCooldown => _jumpCooldown;
        public float JumpInputTimeBufferSize => _jumpInputTimeBufferSize;
        [HideInInspector] public Rigidbody2D JumpRigidbody;

        [SerializeField] private float _jumpForce = 10;
        [SerializeField] private float _jumpCooldown = 0.1f;
        [SerializeField] private float _jumpInputTimeBufferSize = 0.1f;
    }
}
