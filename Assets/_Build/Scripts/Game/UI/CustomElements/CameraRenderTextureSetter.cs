using UnityEngine;

namespace LostKaiju.Game.UI.CustomElements
{
    public class CameraRenderTextureSetter : MonoBehaviour
    {
        public RenderTexture CurrentRenderTexture { get; private set; }

        [SerializeField] private Camera _camera;
        [SerializeField] private RenderTexture _renderTextureAsset;

        public void Init()
        {
            var newRenderTexture = new RenderTexture(_renderTextureAsset.descriptor)
            {
                name = $"{name} - RenderTexture"
            };
            if (_camera.targetTexture != null)
            {
                _camera.targetTexture.Release();
            }
            _camera.targetTexture = newRenderTexture;
            CurrentRenderTexture = newRenderTexture;
        }
    }
}