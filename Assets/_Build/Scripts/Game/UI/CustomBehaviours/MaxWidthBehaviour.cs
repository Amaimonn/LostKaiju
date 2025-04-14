using UnityEngine;
using UnityEngine.EventSystems;

namespace TrinketShop.Game.UI.CustomBehaviours
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class MaxWidthBehaviour : UIBehaviour
    {
        [SerializeField, Min(0)] private float _maxWidth = 1600f;
        [SerializeField] private RectTransform _rectTransform;

        private RectTransform _parentRect;

        protected override void OnEnable()
        {
            base.OnEnable();
            CacheRefs();
            ApplyWidth();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            ApplyWidth();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            CacheRefs();
            ApplyWidth();
        }
#endif

        private void CacheRefs()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform.parent is RectTransform parent)
                _parentRect = parent;
        }

        private void ApplyWidth()
        {
            if (_rectTransform == null || _parentRect == null)
                return;

            var parentWidth = _parentRect.rect.width;

            var targetWidth = Mathf.Min(parentWidth, _maxWidth);

            if (!Mathf.Approximately(_rectTransform.rect.width, targetWidth))
            {
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
            }
        }
    }
}
