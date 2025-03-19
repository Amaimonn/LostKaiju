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
        public Observable<float> OverlayFillProgress => _overlayFillProgress;

        [SerializeField] private GameObject _loadingGameObject;
        [SerializeField] private Image _overlayImage;
        [SerializeField] private GameObject _loadingLabel;
        [SerializeField] private string _overlayProgressProperty;
        [SerializeField, Min(0.01f)] private float _overlayFillSeconds = 2f;

        private readonly Subject<Unit> _onStarted = new();
        private readonly Subject<Unit> _onFinished = new();
        private readonly ReactiveProperty<float> _overlayFillProgress = new(1);


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

            while (_overlayFillProgress.Value < 1)
            {
                var currentProgress = _overlayFillProgress.Value + Time.deltaTime / _overlayFillSeconds;
                if (currentProgress > 1)
                    currentProgress = 1;
                SetOverlayFillProgress(currentProgress);
                yield return null;
            }

            _loadingLabel.SetActive(true);
        }

        public IEnumerator HideCoroutine()
        {
            _loadingLabel.SetActive(false);

            while (_overlayFillProgress.Value > 0)
            {
                var currentProgress = _overlayFillProgress.Value - Time.deltaTime / _overlayFillSeconds;
                if (currentProgress < 0)
                    currentProgress = 0;
                SetOverlayFillProgress(currentProgress);
                yield return null;
            }

            _loadingGameObject.SetActive(false);
            _onFinished.OnNext(Unit.Default);
        }

        private void SetOverlayFillProgress(float progress)
        {
            _overlayFillProgress.Value = progress;
            _overlayImage.material.SetFloat(_overlayProgressProperty, _overlayFillProgress.Value);
        }
    }
}