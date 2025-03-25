using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using R3;

namespace LostKaiju.Services.Inputs
{
    public class InputSystemProvider : IInputProvider
    {
        public float GetHorizontal // raw axis value
        {
            get 
            {
                var readValueX = _onReadMove().x; 
                
                if (readValueX >= SENSITIVITY )
                {
                    _horizontalCanceled.Value = false;
                    return 1;
                }
                else if (readValueX < -SENSITIVITY )
                {
                    _horizontalCanceled.Value = false;
                    return -1;
                }
                else
                {
                    _horizontalCanceled.Value = true;
                    return 0;
                }
            }
        }

        public Observable<bool> OnHorizontalCanceled => _horizontalCanceled;

        public float GetVertical
        {
            get
            {
                var readValueY = _onReadMove().y; 
                
                if (readValueY >= SENSITIVITY )
                {
                    _verticalCanceled.Value = false;
                    return 1;
                }
                else if (readValueY < -SENSITIVITY )
                {
                    _verticalCanceled.Value = false;
                    return -1;
                }
                else
                {
                    _verticalCanceled.Value = true;
                    return 0;
                }
            }
        }

        public Observable<bool> OnVerticalCanceled => _verticalCanceled;

        public bool GetJump => _onReadJump();

        public bool GetShift => _onReadDash();

        public bool GetAttack => _onReadAttack();
        public Observable<Unit> OnEscape => _onEscape;
        public Observable<Unit> OnOptions => _onOptions;

        private const float SENSITIVITY = 0.5f;
        private readonly Func<Vector2> _onReadMove;
        private readonly Func<bool> _onReadJump;
        private readonly Func<bool> _onReadDash;
        private readonly Func<bool> _onReadAttack;
        private readonly ReactiveProperty<bool> _horizontalCanceled = new(true);
        private readonly ReactiveProperty<bool> _verticalCanceled = new(true);
        private readonly Subject<Unit> _onEscape = new();
        private readonly Subject<Unit> _onOptions = new();

        public InputSystemProvider()
        {
            var moveAction = InputSystem.actions.FindAction("Move");
            var jumpAction = InputSystem.actions.FindAction("Jump");
            var dashAction = InputSystem.actions.FindAction("Dash");
            var attackAction = InputSystem.actions.FindAction("Attack");
            var attackButtonAction = InputSystem.actions.FindAction("AttackButton");
            // InputSystem.actions.FindAction("Cancel").started += _ => _onEscape.OnNext(Unit.Default);
            InputSystem.actions.FindAction("Options").started += _ => _onOptions.OnNext(Unit.Default);

            _onReadMove = moveAction.ReadValue<Vector2>;
            _onReadDash = dashAction.WasPressedThisFrame; //() => dashAction.ReadValue<float>() > 0;
            _onReadAttack = () => attackAction.WasPressedThisFrame() && !EventSystem.current.IsPointerOverGameObject()
                || attackButtonAction.WasPressedThisFrame(); 
            _onReadJump = () => jumpAction.ReadValue<float>() > 0 || moveAction.ReadValue<Vector2>().y >= SENSITIVITY;
        }
    }
}