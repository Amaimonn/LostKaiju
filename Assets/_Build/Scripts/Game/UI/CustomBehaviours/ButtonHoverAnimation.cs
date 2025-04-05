using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LostKaiju.Game.UI.CustomBehaviours
{
    [RequireComponent(typeof(Button))]
    public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _hoverScale = 1.1f;
        [SerializeField] private float _duration = 0.1f;

        private Vector3 _originalScale;
        private RectTransform _rectTransform;
        private Coroutine _currentAnimation;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_currentAnimation != null)
            {
                StopCoroutine(_currentAnimation);
            }
            _currentAnimation = StartCoroutine(AnimateScale(_originalScale * _hoverScale));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_currentAnimation != null)
            {
                StopCoroutine(_currentAnimation);
            }
            _currentAnimation = StartCoroutine(AnimateScale(_originalScale));
        }

        private IEnumerator AnimateScale(Vector3 targetScale)
        {
            Vector3 startScale = _rectTransform.localScale;
            var elapsedTime = 1 - Mathf.Abs(targetScale.x - startScale.x) / _duration;

            while (elapsedTime < _duration)
            {
                _rectTransform.localScale = Vector3.Lerp(
                    startScale,
                    targetScale,
                    elapsedTime / _duration
                );
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            _rectTransform.localScale = targetScale;
            _currentAnimation = null;
        }
    }
}