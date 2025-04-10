using UnityEngine;

namespace LostKaiju.Utils
{
    [ExecuteAlways]
    public class CameraAspect : MonoBehaviour
    {
        [Header("Aspect Ratio Limits")]
        [SerializeField] private Vector2 _minAspect = new(16, 9);
        [SerializeField] private Vector2 _maxAspect = new(21, 9);

        private Camera _camera;
        private float _minAspectRatio;
        private float _maxAspectRatio;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            CalculateAspectRatios();
            UpdateCamera();
        }

        private void OnValidate() => CalculateAspectRatios();
        private void Update() => UpdateCamera();

        private void CalculateAspectRatios()
        {
            _minAspectRatio = _minAspect.x / _minAspect.y;
            _maxAspectRatio = _maxAspect.x / _maxAspect.y;
        }

        private void UpdateCamera()
        {
            var currentAspect = Screen.width / (float) Screen.height;

            if (currentAspect > _maxAspectRatio)
            {
                var normalizedWidth = _maxAspectRatio / currentAspect;
                var barThickness = (1f - normalizedWidth) / 2f;
                _camera.rect = new Rect(barThickness, 0, normalizedWidth, 1f);
            }
            else if (currentAspect < _minAspectRatio)
            {
                var normalizedHeight = currentAspect / _minAspectRatio;
                var barThickness = (1f - normalizedHeight) / 2f;
                _camera.rect = new Rect(0, barThickness, 1f, normalizedHeight);
            }
            else
            {
                _camera.rect = new Rect(0, 0, 1, 1);
            }
        }
    }
}