using UnityEngine;

public class ScaleFlipper : Flipper
{
    public override bool IsLooksToTheRight => _isInitialLooksToTheRight == transform.localScale.x > 0;

    [SerializeField] private bool _isInitialLooksToTheRight;

    public override void LookRight(bool isTrue)
    {
        if (IsLooksToTheRight == isTrue)
            return;
            
        var sign = _isInitialLooksToTheRight ? 1 : -1;
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
