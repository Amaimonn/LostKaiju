using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Features
{
    public class ScaleFlipper : Flipper
    {
        public override bool IsLookingToTheRight => _isInitialLookingToTheRight == transform.localScale.x > 0;

        [SerializeField] private bool _isInitialLookingToTheRight;

        public override void LookRight(bool isTrue)
        {
            if (IsLookingToTheRight == isTrue)
                return;
                
            var sign = _isInitialLookingToTheRight ? 1 : -1;
            if (isTrue)
            {
                transform.localScale = new Vector3(sign * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-sign * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
