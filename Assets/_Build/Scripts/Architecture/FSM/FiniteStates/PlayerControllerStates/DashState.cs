using R3;
using UnityEngine;

public class DashState : PlayerControllerState
{
    public ReadOnlyReactiveProperty<bool> IsCompleted => _isCompleted;

    protected DashParameters _parameters;
    protected float _currentDuration;
    protected Vector2 _startPosition;
    protected ReactiveProperty<bool> _isCompleted = new(false);
    protected ReadOnlyReactiveProperty<bool> _isRight;
    protected float _startGravity;
    // flipper
    public void Init(DashParameters parameters, Observable<bool> isRight)
    {
        _parameters = parameters;
        _isRight = isRight.ToReadOnlyReactiveProperty();
    }

    public override void UpdateLogic()
    {
        HandleTransitions();
    }

    public override void Enter()
    {
        base.Enter();
        _isCompleted.Value = false;
        _currentDuration = 0;
        _startPosition = _parameters.rigidBody.position;
        _startGravity = _parameters.rigidBody.gravityScale;
    }

    public override void Exit()
    {
        base.Exit();
        _parameters.rigidBody.linearVelocityX = 0;
        _parameters.rigidBody.gravityScale = _startGravity;
    }

    public override void FixedUpdateLogic()
    {
        if (!_isCompleted.Value)
        {
            Dash();
        }
    }

    private void Dash()
    {
        var eclapsedTime = Time.fixedDeltaTime;
        var direction = _isRight.CurrentValue ? 1 : -1;
        _parameters.rigidBody.linearVelocityY = 0;
        _parameters.rigidBody.linearVelocityX = direction * _parameters.Distance / _parameters.Duration;
        _currentDuration += eclapsedTime;

        if (_currentDuration >= _parameters.Duration)
        {
            _isCompleted.Value = true;
        }
    }

}