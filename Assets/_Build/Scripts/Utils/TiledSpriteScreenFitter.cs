using UnityEngine;

namespace LostKaiju
{
    public class SpriteTileScreenFitter : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;
        private float _spriteSizeY;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _camera = Camera.main;
            if (!_spriteRenderer || !_camera)
            {
                enabled = false;
                return;
            }
            _spriteSizeY = _spriteRenderer.size.y;
        }

        private void Update()
        {
            var cameraWidthInUnits = _camera.orthographicSize * 2f * _camera.aspect;
            _spriteRenderer.size = new Vector2(cameraWidthInUnits, _spriteSizeY);
        }
    }
}
