using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay.EnemyCreature
{
    public class EnemyHealthWorldView : MonoBehaviour, IView<HealthViewModel>
    {
        [SerializeField] private WorldSpaceUIDocument _worldUI;
        [SerializeField] private string _maskElementName;
        [SerializeField] private string _maskLateElementName;

        private HealthViewModel _viewModel;
        private VisualElement _mask;
        private VisualElement _maskLate;
        private StyleList<StylePropertyName> _maskLateTransitionProperty;
        private StyleList<TimeValue> _maskLateTransitionDuration;
        private StyleList<StylePropertyName> _emptyProperty = new() {};
        private StyleList<TimeValue> _emptyDuration = new() {};
        private bool _lateMaskTransitionsSet;

        public void Bind(HealthViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.HealthFillAmount.Skip(1).Take(1).Subscribe(x =>
            {
                InitUI();
                _viewModel.HealthFillAmount.DelayFrame(2).Subscribe(OnFillAmountChanged);
            });
        }

        private void InitUI()
        {
            _worldUI.Init();
            _mask = _worldUI.Root.Q<VisualElement>(name: _maskElementName);
            _maskLate = _worldUI.Root.Q<VisualElement>(name: _maskLateElementName);
            var maxPercent = Length.Percent(100);
            _mask.style.width = maxPercent;
            _maskLate.style.width = maxPercent;
            _maskLateTransitionProperty = _mask.style.transitionProperty;
            _maskLateTransitionDuration = _mask.style.transitionDuration;
            _lateMaskTransitionsSet = true;
        }

        private void OnFillAmountChanged(float newAmount)
        {
            var newLength = Length.Percent(newAmount * 100.0f);
            var oldLateLength = _maskLate.style.width.value;
            Debug.Log($"new {newLength.value}, old {oldLateLength.value}");
            if (newLength.value > oldLateLength.value)
            {
                _maskLate.style.transitionProperty = _emptyProperty;
                _maskLate.style.transitionDuration = _emptyDuration;
                _lateMaskTransitionsSet = false;
            }
            else if (!_lateMaskTransitionsSet)
            {
                _maskLate.style.transitionProperty = _maskLateTransitionProperty;
                _maskLate.style.transitionDuration = _maskLateTransitionDuration;
                _lateMaskTransitionsSet = true;
            }

            _mask.style.width = newLength;
            _maskLate.style.width = newLength;
        }
    }
}