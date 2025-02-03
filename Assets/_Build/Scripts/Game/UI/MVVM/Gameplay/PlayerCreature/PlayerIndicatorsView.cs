using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay.PlayerCreature
{
    public class PlayerIndicatorsView : CanvasView<PlayerIndicatorsViewModel>
    {
        [SerializeField] private Image _healthBarImage;
        [SerializeField] private TMP_Text _maxHealthText;
        [SerializeField] private TMP_Text _currentHealthText;

        protected override void OnBind(PlayerIndicatorsViewModel viewModel)
        {
            viewModel.MaxHealth.Subscribe(OnMaxHealthSet);
            viewModel.CurrentHealth.Subscribe(OnCurrentHealthSet);
            viewModel.HealthFillAmount.Subscribe(OnHealthFillAmountSet);
        }

        private void OnMaxHealthSet(int amount)
        {
            _maxHealthText.text = amount.ToString();
        }

        private void OnCurrentHealthSet(int amount)
        {
            _currentHealthText.text = amount.ToString();
        }

        private void OnHealthFillAmountSet(float amount)
        {
            _healthBarImage.fillAmount = amount;
        }
    }
}