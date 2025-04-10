using UnityEngine;

namespace LostKaiju.Utils
{
    [ExecuteAlways]
    public class GameAspect : MonoBehaviour
    {
        [Header("Aspect Ratio Limits")]
        [SerializeField] private Vector2 _minAspect = new(16, 9);
        [SerializeField] private Vector2 _maxAspect = new(21, 9);

        private float _minAspectRatio;
        private float _maxAspectRatio;

        private void Awake()
        {
            CalculateAspectRatios();
            EnforceAspectRatio();
        }

        private void OnValidate() => CalculateAspectRatios();
        private void Update() => EnforceAspectRatio();

        private void CalculateAspectRatios()
        {
            _minAspectRatio = _minAspect.x / _minAspect.y;
            _maxAspectRatio = _maxAspect.x / _maxAspect.y;
        }

        private void EnforceAspectRatio()
        {
            float currentAspect = (float)Screen.width / Screen.height;

            if (currentAspect > _maxAspectRatio)
            {
                var newHeight = Screen.height;
                var newWidth = Mathf.RoundToInt(newHeight * _maxAspectRatio);
                Screen.SetResolution(newWidth, newHeight, FullScreenMode.FullScreenWindow);
            }
            else if (currentAspect < _minAspectRatio)
            {
                var newWidth = Screen.width;
                var newHeight = Mathf.RoundToInt(newWidth / _minAspectRatio);
                Screen.SetResolution(newWidth, newHeight, FullScreenMode.FullScreenWindow);
            }
        }
    }
}