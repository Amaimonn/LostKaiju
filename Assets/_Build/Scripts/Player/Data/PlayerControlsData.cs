using System;
using UnityEngine;

using LostKaiju.Player.Data.StateParameters;

namespace LostKaiju.Player.Data
{
    [Serializable]
    public class PlayerControlsData
    {
        public WalkParameters Walk => _walk;
        public JumpParameters Jump => _jump;
        public float JumpForce => _jumpForce;
        public LayerMask GroundMask => _groundMask;
        public Vector2 GroundCheckSize => _groundCheckSize;
        public float JumpCooldown => _jumpCooldown;
        public float JumpInputTimeBufferSize => _jumpInputTimeBufferSize;

        [SerializeField] private WalkParameters _walk;
        [SerializeField] private JumpParameters _jump;
        [SerializeField] private float _jumpForce = 10;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Vector2 _groundCheckSize;
        [SerializeField] private float _jumpCooldown = 0.1f;
        [SerializeField] private float _jumpInputTimeBufferSize = 0.1f;
    }
}
