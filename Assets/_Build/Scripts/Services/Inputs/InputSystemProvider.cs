using System;
using UnityEngine;
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

        private const float SENSITIVITY = 0.2f;
        private readonly Func<Vector2> _onReadMove;
        private readonly Func<bool> _onReadJump;
        private readonly Func<bool> _onReadDash;
        private readonly Func<bool> _onReadAttack;
        private readonly ReactiveProperty<bool> _horizontalCanceled = new(true);
        private readonly ReactiveProperty<bool> _verticalCanceled = new(true);
        private readonly Subject<Unit> _onEscape = new();
        private readonly Subject<Unit> _onOptions = new();
        private readonly KaijuInputActions _playerInputSystem = new();

        public InputSystemProvider()
        {
            _playerInputSystem.Enable();
            
            var gameplayActions = _playerInputSystem.Player;
            gameplayActions.Options.started += _ => _onOptions.OnNext(Unit.Default);

            _onReadMove = gameplayActions.Move.ReadValue<Vector2>;
            _onReadDash = gameplayActions.Dash.WasPressedThisFrame; //() => dashAction.ReadValue<float>() > 0;

            _onReadAttack = () => gameplayActions.Attack.WasPressedThisFrame() && 
                !EventSystem.current.IsPointerOverGameObject() || 
                gameplayActions.AttackButton.WasPressedThisFrame(); 

            _onReadJump = () => gameplayActions.Jump.ReadValue<float>() > 0 ||
                gameplayActions.Move.ReadValue<Vector2>().y >= SENSITIVITY;

            // TEST
            gameplayActions.Attack.started += _ => Debug.Log("Click");
        }
    }
}