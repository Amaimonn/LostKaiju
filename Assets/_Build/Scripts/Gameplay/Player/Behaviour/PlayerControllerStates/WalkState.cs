using UnityEngine;
using R3;

using LostKaiju.Gameplay.Player.Data.StateParameters;

namespace LostKaiju.Gameplay.Player.Behaviour.PlayerControllerStates
{
    public class WalkState : PlayerControllerState
    {
        public Observable<bool> IsPositiveDirectionX => _isPositiveDirectionX;

        protected ReactiveProperty<bool> _isPositiveDirectionX = new();
        protected WalkParameters _parameters;
        protected Rigidbody2D _rigidbody;
        // protected Func<bool> _checkIsGrounded;
        private float _readHorizontal;

        public void Init(WalkParameters parameters, Rigidbody2D rigidbody) //, Func<bool> checkIsGrounded
        {
            _parameters = parameters;
            _rigidbody = rigidbody;
            // _checkIsGrounded = checkIsGrounded;
        }

        public override void UpdateLogic()
        {
            _readHorizontal = InputProvider.GetHorizontal;

            if (_readHorizontal != 0)
                _isPositiveDirectionX.Value = _readHorizontal > 0;

            HandleTransitions();
        }

        public override void FixedUpdateLogic()
        {
            if (_readHorizontal != 0)
                Walk(_readHorizontal);
        }

        private void Walk(float xMagnitude)
        {
            // var airResistanceMultiplier = 1;//_checkIsGrounded() ? 1f : _parameters.AirMultiplier;
            var clearSpeed = xMagnitude * _parameters.WalkSpeed;// * airResistanceMultiplier;
            var speedDifference = clearSpeed - _rigidbody.linearVelocityX;
            var definedAcceleration = Mathf.Abs(clearSpeed) > 0.01f ? _parameters.Acceleration : _parameters.Deceleration;
            
            _rigidbody.AddForceX(definedAcceleration * speedDifference);
        }
    }
}
