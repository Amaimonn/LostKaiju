using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.UI.MVVM.Gameplay.MissionResults
{
    public class MissionResultsView : PopUpCanvasView<MissionResultsViewModel>
    {
        [Header("Panel"), Space(4)]
        [SerializeField] private RectTransform _panelRectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Mission Info"), Space(4)]
        [SerializeField] private TMP_Text _missionName;
        [SerializeField] private TMP_Text _locationName;

        [Header("Stars"), Space(4)]
        [SerializeField] private Image _star1;
        [SerializeField] private Image _star2;
        [SerializeField] private Image _star3;
        [SerializeField] private Color _awardedStarColor = Color.white;
        [SerializeField] private Color _lockedStarColor = Color.gray;
        [SerializeField] private float _starAnimationDuration = 0.5f;
        [SerializeField] private float _delayBetweenStars = 0.3f;

        [Header("Conditions"), Space(4)]
        [SerializeField] private TMP_Text _condition1;
        [SerializeField] private TMP_Text _condition2;
        [SerializeField] private TMP_Text _condition3;
        [SerializeField] private Color _awardedConditionColor = Color.white;
        [SerializeField] private Color _lockedConditionColor = Color.gray;

        [Header("Button"), Space(4)]
        [SerializeField] private float _buttonAppearDelay = 0.5f;
        [SerializeField] private float _buttonAnimationDuration = 0.3f;

        [Header("Animation"), Space(4)]
        [SerializeField] private float _openAnimationDuration = 0.7f;
        [SerializeField] private AnimationCurve _openAnimationCurve;

        [Header("SFX"), Space(4)]
        [SerializeField] private AudioClip _closingSFX;
        [SerializeField] private AudioClip _starAwardedSFX;
        [SerializeField] private AudioClip _buttonAppearSFX;

        private RectTransform _buttonRectTransform;
        private Vector2 _initialPanelScale;
        private Vector2 _initialButtonScale;

        protected override CanvasOrder Order => CanvasOrder.Last;

#region MonoBehaviour
        private void Awake()
        {
            _initialPanelScale = _panelRectTransform.localScale;
            
            _buttonRectTransform = _closeButton.GetComponent<RectTransform>();
            _initialButtonScale = _buttonRectTransform.localScale;
            
            _closeButton.gameObject.SetActive(false);
            _canvasGroup.alpha = 0f;
        }
#endregion

#region PopUpCanvasView
        protected override void OnBind(MissionResultsViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            _star1.color = _lockedStarColor;
            _star2.color = _lockedStarColor;
            _star3.color = _lockedStarColor;
            
            _condition1.color = _lockedConditionColor;
            _condition2.color = _lockedConditionColor;
            _condition3.color = _lockedConditionColor;

            _missionName.SetText(new LocalizedString(Tables.CAMPAIGN, viewModel.MissionData.Name).GetLocalizedString());
            _locationName.SetText(new LocalizedString(Tables.CAMPAIGN, viewModel.LocationData.Name).GetLocalizedString());
        }

        protected override void OnOpening()
        {
            StartCoroutine(OpenAnimationCoroutine());
        }
#endregion

        private IEnumerator OpenAnimationCoroutine()
        {
            yield return AnimatePanelScale();

            yield return AnimateFade();

            if (ViewModel.FirstStarAwarded.CurrentValue)
            {
                yield return AnimateStar(_star1, _condition1);
            }

            if (ViewModel.SecondStarAwarded.CurrentValue)
            {
                yield return new WaitForSeconds(_delayBetweenStars);
                yield return AnimateStar(_star2, _condition2);
            }

            if (ViewModel.ThirdStarAwarded.CurrentValue)
            {
                yield return new WaitForSeconds(_delayBetweenStars);
                yield return AnimateStar(_star3, _condition3);
            }

            yield return new WaitForSeconds(_buttonAppearDelay);
            
            yield return AnimateButtonAppearance();
        }

        private IEnumerator AnimatePanelScale()
        {
            var timer = 0f;
            while (timer < _openAnimationDuration)
            {
                var progress = _openAnimationCurve.Evaluate(timer / _openAnimationDuration);
                _panelRectTransform.localScale = _initialPanelScale * progress;
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            _panelRectTransform.localScale = _initialPanelScale;
        }

        private IEnumerator AnimateFade()
        {
            var timer = 0f;
            while (timer < _openAnimationDuration)
            {
                var progress = _openAnimationCurve.Evaluate(timer / _openAnimationDuration);
                _canvasGroup.alpha = progress;
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            _canvasGroup.alpha = 1f;
        }

        private IEnumerator AnimateStar(Image star, TMP_Text condition)
        {
            var timer = 0f;
            var startColor = star.color;
            
            _audioPlayer.PlaySFX(_starAwardedSFX);
            condition.color = _awardedConditionColor;
            
            while (timer < _starAnimationDuration)
            {
                star.color = Color.Lerp(startColor, _awardedStarColor, timer / _starAnimationDuration);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            star.color = _awardedStarColor;
        }

        private IEnumerator AnimateButtonAppearance()
        {
            _audioPlayer.PlaySFX(_buttonAppearSFX);
            _closeButton.gameObject.SetActive(true);
            
            var timer = 0f;
            var startSize = _initialButtonScale * 0.5f;
            var endSize = _initialButtonScale;
            
            _buttonRectTransform.localScale = startSize;
            
            while (timer < _buttonAnimationDuration)
            {
                _buttonRectTransform.localScale = Vector2.Lerp(startSize, endSize, timer / _buttonAnimationDuration);
                
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            
            _buttonRectTransform.localScale = endSize;
        }

        protected override void OnClosing()
        {
            PlayClosingSFX();
            base.OnClosing();
        }

        private void PlayClosingSFX()
        {
            _audioPlayer.PlaySFX(_closingSFX);
        }
    }
}