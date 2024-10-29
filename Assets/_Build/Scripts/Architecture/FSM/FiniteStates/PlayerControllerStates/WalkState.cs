using UnityEngine;
using R3;
using System;

public class WalkState : PlayerControllerState
{
    public Observable<bool> IsPositiveDirectionX => _isPositiveDirectionX;

    protected ReactiveProperty<bool> _isPositiveDirectionX = new();
    protected WalkParameters _parameters;
    // protected ReadOnlyReactiveProperty<bool> _isGrounded;
    protected Func<bool> _checkIsGrounded;
    private Vector2 _readWalk;

    public void Init(WalkParameters parameters, Func<bool> checkIsGrounded)
    {
        _parameters = parameters;
        _checkIsGrounded = checkIsGrounded;
        //_isGrounded = isGrounded.ToReadOnlyReactiveProperty();
    }

    public override void UpdateLogic()
    {
        var horizontalInput = InputProvider.GetHorizontal;
        _readWalk = new Vector2 (horizontalInput, 0);

        if (horizontalInput != 0)
            _isPositiveDirectionX.Value = horizontalInput > 0;

        HandleTransitions();
    }

    public override void FixedUpdateLogic()
    {
        if (_readWalk.magnitude > 0)
            Walk(_readWalk);
    }

    private void Walk(Vector2 direction)
    {
        // var airResistanceMultiplier = 1;//_checkIsGrounded() ? 1f : _parameters.AirMultiplier;
        var clearSpeed = direction.x * _parameters.WalkSpeed;// * airResistanceMultiplier;
        var speedDifference = clearSpeed - _parameters.WalkRigidbody.linearVelocityX;
        var definedAcceleration = Mathf.Abs(clearSpeed) > 0.01f ? _parameters.Acceleration : _parameters.Deceleration;
        _parameters.WalkRigidbody.AddForceX(definedAcceleration * speedDifference);
    }
}