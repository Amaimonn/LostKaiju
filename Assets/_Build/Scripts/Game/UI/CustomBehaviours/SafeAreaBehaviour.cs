using UnityEngine;
using UnityEngine.EventSystems;

namespace LostKaiju.Game.UI.CustomBehaviours
{
    public class SafeAreaBehaviour : UIBehaviour
    {
        [Header("Safe Area")]
        [SerializeField, Min(0)] private float _topScale = 1f;
        [SerializeField, Min(0)] private float _rightScale = 1f;
        [SerializeField, Min(0)] private float _bottomScale = 1f;
        [SerializeField, Min(0)] private float _leftScale = 1f;

        [Header("Minimum Inset (in reference resolution units)")]
        [SerializeField, Min(0)] private float _minTopInset = 0f;
        [SerializeField, Min(0)] private float _minRightInset = 0f;
        [SerializeField, Min(0)] private float _minBottomInset = 0f;
        [SerializeField, Min(0)] private float _minLeftInset = 0f;

        private static Vector2 ReferenceResolution = new(1920, 1080);

        private RectTransform _rectTransform;
        private Rect _lastSafeArea;
        private Vector2 _lastScreenSize;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            ApplySafeAreaIfNeeded();
        }

        protected override void Start()
        {
            base.Start();
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
                ApplySafeArea(screenSize);
            }
        }

        private void ApplySafeArea(Vector2 screenSize)
        {
            var safeMin = _lastSafeArea.min;
            var safeMax = _lastSafeArea.max;

            var screenWidth = screenSize.x;
            var screenHeight = screenSize.y;

            // Calculate the scale factor based on the current screen size vs reference resolution
            var widthRatio = screenWidth / ReferenceResolution.x;
            var heightRatio = screenHeight / ReferenceResolution.y;
            var scaleFactor = Mathf.Min(widthRatio, heightRatio);
            // Or use Mathf.Lerp(widthRatio, heightRatio, 0.5f) for mixed scaling

            // Scale the minimum insets according to the reference resolution
            var scaledMinTopInset = _minTopInset * scaleFactor;
            var scaledMinRightInset = _minRightInset * scaleFactor;
            var scaledMinBottomInset = _minBottomInset * scaleFactor;
            var scaledMinLeftInset = _minLeftInset * scaleFactor;

            // Calculate left bound with scale and minimum inset
            var leftBound = safeMin.x;
            if (_leftScale > 0)
                leftBound = Mathf.Lerp(0, safeMin.x, _leftScale);
            leftBound = Mathf.Max(leftBound, scaledMinLeftInset);

            // Calculate right bound with scale and minimum inset
            var rightBound = safeMax.x;
            if (_rightScale > 0)
                rightBound = Mathf.Lerp(screenWidth, safeMax.x, _rightScale);
            rightBound = Mathf.Min(rightBound, screenWidth - scaledMinRightInset);

            // Calculate bottom bound with scale and minimum inset
            var bottomBound = safeMin.y;
            if (_bottomScale > 0)
                bottomBound = Mathf.Lerp(0, safeMin.y, _bottomScale);
            bottomBound = Mathf.Max(bottomBound, scaledMinBottomInset);

            // Calculate top bound with scale and minimum inset
            var topBound = safeMax.y;
            if (_topScale > 0)
                topBound = Mathf.Lerp(screenHeight, safeMax.y, _topScale);
            topBound = Mathf.Min(topBound, screenHeight - scaledMinTopInset);

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
}