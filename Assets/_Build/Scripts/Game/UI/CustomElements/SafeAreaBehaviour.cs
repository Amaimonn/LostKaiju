using UnityEngine;
using UnityEngine.EventSystems;

public class SafeAreaBehaviour : UIBehaviour
{
    [Header("Safe Area")]
    [SerializeField, Min(0)] private float _topScale = 1f;
    [SerializeField, Min(0)] private float _rightScale = 1f;
    [SerializeField, Min(0)] private float _bottomScale = 1f;
    [SerializeField, Min(0)] private float _leftScale = 1f;

    [Header("Minimum Inset")]
    [SerializeField, Min(0)] private float _minTopInset = 0f;
    [SerializeField, Min(0)] private float _minRightInset = 0f;
    [SerializeField, Min(0)] private float _minBottomInset = 0f;
    [SerializeField, Min(0)] private float _minLeftInset = 0f;

    private RectTransform _rectTransform;
    private Rect _lastSafeArea;
    private Vector2 _lastScreenSize;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        ApplySafeAreaIfNeeded();
    }

    private void ApplySafeAreaIfNeeded()
    {
        var safeArea = Screen.safeArea;
        var screenSize = new Vector2(Screen.width, Screen.height);

        if (safeArea != _lastSafeArea || screenSize != _lastScreenSize)
        {
            if (_rectTransform == null) 
                _rectTransform = GetComponent<RectTransform>();
            _lastSafeArea = safeArea;
            _lastScreenSize = screenSize;
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        var safeMin = _lastSafeArea.min;
        var safeMax = _lastSafeArea.max;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate left bound with scale and minimum inset
        float leftBound = safeMin.x;
        if (_leftScale > 0)
            leftBound = Mathf.Lerp(0, safeMin.x, _leftScale);
        leftBound = Mathf.Max(leftBound, _minLeftInset);

        // Calculate right bound with scale and minimum inset
        float rightBound = safeMax.x;
        if (_rightScale > 0)
            rightBound = Mathf.Lerp(screenWidth, safeMax.x, _rightScale);
        rightBound = Mathf.Min(rightBound, screenWidth - _minRightInset);

        // Calculate bottom bound with scale and minimum inset
        float bottomBound = safeMin.y;
        if (_bottomScale > 0)
            bottomBound = Mathf.Lerp(0, safeMin.y, _bottomScale);
        bottomBound = Mathf.Max(bottomBound, _minBottomInset);

        // Calculate top bound with scale and minimum inset
        float topBound = safeMax.y;
        if (_topScale > 0)
            topBound = Mathf.Lerp(screenHeight, safeMax.y, _topScale);
        topBound = Mathf.Min(topBound, screenHeight - _minTopInset);

        // Ensure bounds are valid (left < right, bottom < top)
        leftBound = Mathf.Min(leftBound, rightBound - 1); // -1 to ensure at least 1 pixel width
        bottomBound = Mathf.Min(bottomBound, topBound - 1); // -1 to ensure at least 1 pixel height

        _rectTransform.anchorMin = new Vector2(
            leftBound / screenWidth,
            bottomBound / screenHeight
        );
        _rectTransform.anchorMax = new Vector2(
            rightBound / screenWidth,
            topBound / screenHeight
        );
    }
}