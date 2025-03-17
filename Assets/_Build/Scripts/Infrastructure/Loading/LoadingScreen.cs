using System.Collections;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LostKaiju.Infrastructure.Loading
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreenNotifier
    {
#region ILoadingScreenNotifier
        public Observable<Unit> OnStarted => _onStarted;
        public Observable<Unit> OnFinished => _onFinished;
#endregion

        [SerializeField] private GameObject _loadingGameObject;
        [SerializeField] private Image _overlayImage;
        [SerializeField] private GameObject _loadingLabel;
        [SerializeField] private string _overlayProgressProperty;
        [SerializeField, Min(0.01f)] private float _overlayFillSeconds = 2f;

        private readonly Subject<Unit> _onStarted = new();
        private readonly Subject<Unit> _onFinished = new();
        private float _overlayFillProgress = 1;


#region MonoBehaviour
        private void Awake()
        {
            Hide();
        }
#endregion

        public void Show()
        {
            _loadingGameObject.SetActive(true);
            _loadingLabel.SetActive(true);
            SetOverlayFillProgress(1);
            _onStarted.OnNext(Unit.Default);
        }

        public void Hide()
        {
            _loadingLabel.SetActive(false);
            _loadingGameObject.SetActive(false);
            SetOverlayFillProgress(0);
            _onFinished.OnNext(Unit.Default);
        }

        public IEnumerator ShowCoroutine()
        {
            _onStarted.OnNext(Unit.Default);
            _loadingGameObject.SetActive(true);

            while (_overlayFillProgress < 1)
            {
                SetOverlayFillProgress(_overlayFillProgress + Time.deltaTime / _overlayFillSeconds);
                yield return null;
            }

            SetOverlayFillProgress(1);
            _loadingLabel.SetActive(true);
        }

        public IEnumerator HideCoroutine()
        {
            _loadingLabel.SetActive(false);

            while (_overlayFillProgress > 0)
            {
                SetOverlayFillProgress(_overlayFillProgress - Time.deltaTime / _overlayFillSeconds);
                yield return null;
            }

            SetOverlayFillProgress(0);
            _loadingGameObject.SetActive(false);
            _onFinished.OnNext(Unit.Default);
        }

        private void SetOverlayFillProgress(float progress)
        {
            _overlayFillProgress = progress;
            _overlayImage.material.SetFloat(_overlayProgressProperty, _overlayFillProgress);
        }
    }
}