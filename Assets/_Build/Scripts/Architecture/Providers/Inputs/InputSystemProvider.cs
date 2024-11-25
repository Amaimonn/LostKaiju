using UnityEngine;
using R3;

using LostKaiju.Architecture.Providers.Inputs;
using LostKaiju.Architecture.Providers.Inputs.Inputs;
using UnityEngine.InputSystem;

public class InputSystemProvider : IInputProvider
{
    public float GetHorizontal => _readMovement.x;

    public Observable<bool> OnHorizontalCanceled => _horizontalCanceled;

    public float GetVertical
    {
        get
        {
            var readValueY = _playerInput.Player.Move.ReadValue<Vector2>().y;
            _verticalCanceled.Value = readValueY == 0;
            return _readMovement.y;
        }
    }

    public Observable<bool> OnVerticalCanceled => _verticalCanceled;

    public bool GetJump => _readJump;

    public bool GetShift => _playerInput.Player.Sprint.ReadValue<bool>();

    public InputSystemProvider()
    {
        _playerInput = new();
        var moveAction = InputSystem.actions.FindAction("Move");
        var jumpAction = InputSystem.actions.FindAction("Jump");

        moveAction.performed += ctx => {
            _readMovement = ctx.ReadValue<Vector2>();
            var readX = _readMovement.x;

            if (readX > 0)
                _readMovement.x = 1;
            else if (readX < 0)
                _readMovement.x = -1;

            _horizontalCanceled.Value= _readMovement.x == 0;
            _verticalCanceled.Value = _readMovement.y == 0;
        };
        moveAction.canceled += ctx => {
            _readMovement = Vector2.zero; 
        };
        
        if (SystemInfo.deviceType == DeviceType.Handheld || true)
        {
            moveAction.performed += ctx => {
                _readJump = ctx.ReadValue<Vector2>().y > 0.5f;
            };
            moveAction.canceled += ctx => {
                _readJump = false;
            };
        }
        else
        {
            jumpAction.performed += ctx => {
                _readJump = true;
            };
            jumpAction.canceled += ctx => {
                _readJump = false; 
            };
        }
// #if UNITY_EDITOR
//             moveAction.performed += ctx => {
//                 _readJump = ctx.ReadValue<Vector2>().y > 0.5f;
//             };
//             moveAction.canceled += ctx => {_readJump = false; };
// #endif
    }

    private PlayerInputActions _playerInput;
    private Vector2 _readMovement;
    private bool _readJump;
    private ReactiveProperty<bool> _horizontalCanceled = new(true);
    private ReactiveProperty<bool> _verticalCanceled = new(true);

    // private Observable<bool> _moveCanceled = Observable.Create<bool>(sub => {
    //     Action<UnityEngine.InputSystem.InputAction.CallbackContext> subObserver = (e) => sub.OnNext(true);
    //     _playerInput.Player.Move.canceled += subObserver;
    //     return Disposable.Create(() => _playerInput.Player.Move.canceled -= subObserver);
    // });
}