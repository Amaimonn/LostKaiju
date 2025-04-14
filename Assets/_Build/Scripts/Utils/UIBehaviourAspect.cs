using UnityEngine;
using UnityEngine.EventSystems;

namespace LostKaiju.Utils
{
    [ExecuteAlways]
    public class UIBehaviourAspect : UIBehaviour
    {
        [Header("Aspect Ratio Limits")]
        [SerializeField] private Vector2 _minAspect = new(16, 9);
        [SerializeField] private Vector2 _maxAspect = new(21, 9);

        private float _minAspectRatio;
        private float _maxAspectRatio;
        private RectTransform _selfRectTransform;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (!_selfRectTransform)
            {
                _selfRectTransform = GetComponent<RectTransform>();
                CalculateAspectRatios();
            }
            UpdateRectRatio();
        }

        private void CalculateAspectRatios()
        {
            _minAspectRatio = _minAspect.x / _minAspect.y;
            _maxAspectRatio = _maxAspect.x / _maxAspect.y;
        }

        private void UpdateRectRatio()
        {
            if (_selfRectTransform == null)
                return;

            var currentAspect = Screen.width / (float)Screen.height;

            if (currentAspect > _maxAspectRatio)
            {
                // Too wide - add side bars
                var normalizedWidth = _maxAspectRatio / currentAspect;
                var barThickness = (1f - normalizedWidth) / 2f;

                _selfRectTransform.anchorMin = new Vector2(barThickness, 0);
                _selfRectTransform.anchorMax = new Vector2(1f - barThickness, 1f);
            }
            else if (currentAspect < _minAspectRatio)
            {
                // Too tall - add top/bottom bars
                var normalizedHeight = currentAspect / _minAspectRatio;
                var barThickness = (1f - normalizedHeight) / 2f;

                _selfRectTransform.anchorMin = new Vector2(0, barThickness);
                _selfRectTransform.anchorMax = new Vector2(1f, 1f - barThickness);
            }
            else
            {
                // Perfect aspect ratio
                _selfRectTransform.anchorMin = Vector2.zero;
                _selfRectTransform.anchorMax = Vector2.one;
            }
        }
    }
}