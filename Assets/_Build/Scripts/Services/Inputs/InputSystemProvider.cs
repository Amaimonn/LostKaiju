using System;
using UnityEngine;
using R3;

using UnityEngine.InputSystem;

namespace LostKaiju.Services.Inputs
{
    public class InputSystemProvider : IInputProvider
    {
        public float GetHorizontal 
        {
            get 
            {
                var readValueX = Mathf.Round(_onReadMove().x); // raw axis value
                _horizontalCanceled.Value = readValueX == 0;

                return readValueX;
            }
        }

        public Observable<bool> OnHorizontalCanceled => _horizontalCanceled;

        public float GetVertical
        {
            get
            {
                var readValueY = Mathf.Round(_onReadMove().y); // raw axis value
                _verticalCanceled.Value = readValueY == 0;
                return readValueY;
            }
        }

        public Observable<bool> OnVerticalCanceled => _verticalCanceled;

        public bool GetJump => _onReadJump();

        public bool GetShift => _onReadDash();

        public bool GetAttack => _onReadAttack();

        private readonly Func<Vector2> _onReadMove;
        private readonly Func<bool> _onReadJump;
        private readonly Func<bool> _onReadDash;
        private readonly Func<bool> _onReadAttack;
        private readonly ReactiveProperty<bool> _horizontalCanceled = new(true);
        private readonly ReactiveProperty<bool> _verticalCanceled = new(true);

        public InputSystemProvider()
        {
            var moveAction = InputSystem.actions.FindAction("Move");
            var jumpAction = InputSystem.actions.FindAction("Jump");
            var dashAction = InputSystem.actions.FindAction("Dash");
            var attackAction = InputSystem.actions.FindAction("Attack");

            _onReadMove = moveAction.ReadValue<Vector2>;
            _onReadDash = dashAction.WasPressedThisFrame; //() => dashAction.ReadValue<float>() > 0;
            _onReadAttack = attackAction.WasPressedThisFrame; // () => attackAction.ReadValue<float>() > 0;

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                _onReadJump = () => moveAction.ReadValue<Vector2>().y > 0.5f;
            }
            else
            {
                _onReadJump = () => jumpAction.ReadValue<float>() > 0;
            }
        }

        // private Observable<bool> _moveCanceled = Observable.Create<bool>(sub => {
        //     Action<UnityEngine.InputSystem.InputAction.CallbackContext> subObserver = (e) => sub.OnNext(true);
        //     _playerInput.Player.Move.canceled += subObserver;
        //     return Disposable.Create(() => _playerInput.Player.Move.canceled -= subObserver);
        // });
    }
}