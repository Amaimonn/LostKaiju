using UnityEngine;
using R3;

namespace LostKaiju.Architecture.Providers
{
    public class BaseInputProvider : IInputProvider
    {
        public float GetHorizontal
        {
            get
            {
                var horizontal = Input.GetAxisRaw("Horizontal");
                _horizontalCanceled.Value = horizontal == 0;
                return Input.GetAxisRaw("Horizontal");
            }
        } 

        public Observable<bool> OnHorizontalCanceled => _horizontalCanceled.Skip(1);

        public float GetVertical
        {
            get
            {
                var vertical = Input.GetAxisRaw("Vertical");
                _verticalCanceled.Value = vertical == 0;
                return Input.GetAxisRaw("Vertical");
            }
        } 

        public Observable<bool> OnVerticalCanceled => _verticalCanceled.Skip(1);

        public bool GetJump => Input.GetAxisRaw("Jump") > 0;

        public bool GetShift => Input.GetKeyDown(KeyCode.LeftShift);

        private ReactiveProperty<bool> _horizontalCanceled = new(true);
        private ReactiveProperty<bool> _verticalCanceled = new(true);
    }
}