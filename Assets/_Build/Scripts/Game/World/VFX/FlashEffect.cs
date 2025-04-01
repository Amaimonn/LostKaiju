using System.Collections;
using UnityEngine;

namespace LostKaiju.Game.World.VFX
{
    public class FlashEffect : Effect
    {
        [SerializeField] private Material _flashMaterial;
        [SerializeField] private float _flashDuration = 0.1f;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;

        private static readonly int _flashPropertyId = Shader.PropertyToID("_FlashAmount");
        private Material[] _originalMaterials;
        private bool isFlashing = false;
        private float _currentElapsedTime = 0;

        private void Start()
        {
            _spriteRenderers ??= GetComponentsInChildren<SpriteRenderer>();
            var spritesLength = _spriteRenderers.Length;
            _originalMaterials = new Material[spritesLength];
            for (var i = 0; i < spritesLength; i++)
            {
                _originalMaterials[i] = _spriteRenderers[i].sharedMaterial;
            }
        }

        private void OnDisable()
        {
            if (_spriteRenderers != null && _originalMaterials != null)
            {
                for (var i = 0; i < _spriteRenderers.Length; i++)
                {
                    if (_spriteRenderers[i] != null)
                        _spriteRenderers[i].material = _originalMaterials[i];
                }
            }
        }

        public override void PlayEffect()
        {
            if (!isFlashing && gameObject.activeSelf)
            {
                StartCoroutine(Flash());
            }
            else
            {
                _currentElapsedTime = 0;
            }
        }

        private IEnumerator Flash()
        {
            isFlashing = true;

            foreach (var renderer in _spriteRenderers)
            {
                renderer.material = _flashMaterial;
            }

            _currentElapsedTime = 0f;
            while (_currentElapsedTime < _flashDuration)
            {
                float flashAmount = Mathf.PingPong(_currentElapsedTime / _flashDuration, 1f);
                _flashMaterial.SetFloat(_flashPropertyId, flashAmount);
                _currentElapsedTime += Time.deltaTime;
                yield return null;
            }
            _flashMaterial.SetFloat(_flashPropertyId, 0);

            SetOriginal();

            isFlashing = false;
        }

        private void SetOriginal()
        {
            for (var i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].material = _originalMaterials[i];
            }
        }
    }
}