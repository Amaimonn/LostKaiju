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

        private HealthViewModel _viewModel;
        private VisualElement _mask;

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
            _mask = _worldUI.Root.Q<VisualElement>(_maskElementName);
        }

        private void OnFillAmountChanged(float newAmount)
        {
            _mask.style.width = Length.Percent(newAmount * 100.0f);
        }
    }
}