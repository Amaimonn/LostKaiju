using UnityEngine;
using R3;
using R3.Triggers;

namespace Assets._Build.Scripts.EntityFeatures
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundCheckTrigger : GroundCheck
    {
        public override bool IsGrounded { get => _isGrounded;}
        
        [SerializeField] private Collider2D _groundCheckArea;
        [SerializeField] private LayerMask _groundMask;
        private bool _isGrounded;

        private void Awake()
        {
            _groundCheckArea.OnTriggerEnter2DAsObservable()
                .Where(_ => _groundCheckArea.IsTouchingLayers(_groundMask))
                .Subscribe(e => _isGrounded = true);

            _groundCheckArea.OnTriggerExit2DAsObservable()
                .Where(_ => !_groundCheckArea.IsTouchingLayers(_groundMask))
                .Subscribe(e => _isGrounded = false);
        }
    }
}