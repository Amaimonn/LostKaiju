using UnityEngine;
using R3;

namespace LostKaiju.Services.Inputs
{
    public class SimpleInputProvider : IInputProvider
    {
        public float GetHorizontal
        {
            get
            {
                var horizontal = Input.GetAxisRaw("Horizontal");
                _horizontalCanceled.Value = horizontal == 0;
                return horizontal;
            }
        } 

        public Observable<bool> OnHorizontalCanceled => _horizontalCanceled.Skip(1);

        public float GetVertical
        {
            get
            {
                var vertical = Input.GetAxisRaw("Vertical");
                _verticalCanceled.Value = vertical == 0;
                return vertical;
            }
        } 

        public Observable<bool> OnVerticalCanceled => _verticalCanceled.Skip(1);

        public bool GetJump => Input.GetAxisRaw("Jump") > 0;

        public bool GetShift => Input.GetKey(KeyCode.LeftShift);

        public bool GetAttack => Input.GetKeyDown(KeyCode.Mouse0);

        private readonly ReactiveProperty<bool> _horizontalCanceled = new(true);
        private readonly ReactiveProperty<bool> _verticalCanceled = new(true);
    }
}