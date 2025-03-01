using UnityEngine;
using R3;

using LostKaiju.Game.World.Player.Data.StateParameters;

namespace LostKaiju.Game.World.Player.Behaviour.PlayerControllerStates
{
    public class DashState : PlayerControllerState
    {
        public ReadOnlyReactiveProperty<bool> IsCompleted => _isCompleted;
        public ReadOnlyReactiveProperty<bool> IsRefreshed => _isRefreshed;
        protected DashParameters _parameters;
        protected Rigidbody2D _rigidbody;
        protected float _currentDuration;
        protected Vector2 _startPosition;
        protected ReactiveProperty<bool> _isCompleted = new(true);
        protected ReadOnlyReactiveProperty<bool> _isRight;
        protected ReactiveProperty<bool> _isRefreshed = new(true);
        protected float _startGravity;

        public void Init(DashParameters parameters, Rigidbody2D rigidbody, 
            Observable<bool> isRight, Observable<bool> isRefreshed)
        {
            _parameters = parameters;
            _rigidbody = rigidbody;
            _isRight = isRight.ToReadOnlyReactiveProperty();
            isRefreshed.Where(x => x == true)
                .Subscribe(_ => {
                    _isRefreshed.Value = true;
                    Debug.Log("dash refreshed");
                });
        }

        public override void UpdateLogic()
        {
            HandleTransitions();
        }

        public override void Enter()
        {
            base.Enter();
            _isRefreshed.Value = false;
            _isCompleted.Value = false;
            _currentDuration = 0;
            _startPosition = _rigidbody.position;
            _startGravity = _rigidbody.gravityScale;
            _rigidbody.gravityScale = 0;
        }

        public override void Exit()
        {
            base.Exit();
            _rigidbody.linearVelocityX = 0;
            _rigidbody.gravityScale = _startGravity;
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
            
            _rigidbody.linearVelocityY = 0;
            _rigidbody.linearVelocityX = direction * _parameters.Distance / _parameters.Duration;

            if (_currentDuration >= _parameters.Duration)
            {
                _isCompleted.Value = true;
            }

            _currentDuration += eclapsedTime;
        }

    }
}