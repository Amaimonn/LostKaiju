using UnityEngine;
using R3;

using LostKaiju.Game.World.Player.Data.StateParameters;
using System;

namespace LostKaiju.Game.World.Player.Behaviour.PlayerControllerStates
{
    public class WalkState : PlayerControllerState
    {
        public Observable<bool> IsPositiveDirectionX => _isPositiveDirectionX;

        protected ReactiveProperty<bool> _isPositiveDirectionX = new();
        protected WalkParameters _parameters;
        protected Rigidbody2D _rigidbody;
        private readonly Func<float> _readHorizontalFunc;
        private float _readHorizontal;

        public WalkState(WalkParameters parameters, Rigidbody2D rigidbody, Func<float> readHorizontalFunc)
        {
            _parameters = parameters;
            _rigidbody = rigidbody;
            _readHorizontalFunc = readHorizontalFunc;
        }

        public override void UpdateLogic()
        {
            _readHorizontal = _readHorizontalFunc();

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
            var speedDifference = clearSpeed - _rigidbody.velocity.x;
            var definedAcceleration = Mathf.Abs(clearSpeed) > 0.01f ? _parameters.Acceleration : _parameters.Deceleration;
            
            _rigidbody.AddForce(new Vector2(definedAcceleration * speedDifference, 0), ForceMode2D.Force);
        }
    }
}
