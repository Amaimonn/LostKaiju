using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.Constants;
using LostKaiju.Game.UI.Extentions;
using LostKaiju.Game.GameData.Heroes;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HeroSelectionView : PopUpToolkitView<HeroSelectionViewModel>
    {
        [Header("UIElements")]
        [SerializeField] private string _completeButtonName;
        [SerializeField] private string _heroesListName;
        [SerializeField] private string _heroNameName;
        [SerializeField] private string _heroDescriptionName;
        [SerializeField] private string _selectedImageName;

        [Header("Slots"), Space(4)]
        [SerializeField] private VisualTreeAsset _heroSlot;
        [SerializeField] private string _heroSlotName;
        [SerializeField] private string _heroLabelName;
        [SerializeField] private string _heroImageName;
        [SerializeField] private string _heroSlotSelectedClass;

        [Header("SFX"), Space(4)]
        [SerializeField] private AudioClip _closingSFX; // TODO: another SFX for picking
        [SerializeField] private AudioClip _slotHoverSFX;

        private Button _completeButton;
        private ScrollView _heroesList;
        private Label _heroName;
        private Label _heroDescription;
        private VisualElement _selectedSlot;
        private Dictionary<string, VisualElement> _heroSlotsMap;

        public void SetHeroRenderTexture(RenderTexture heroRenderTexture) // TODO: Provider for RenderTexture 
        {
            var selectedImage = Root.Q<VisualElement>(name: _selectedImageName);
            selectedImage.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(heroRenderTexture));
        }

#region PopUpToolkitView
        protected override void OnAwake()
        {
            base.OnAwake();
            _completeButton = Root.Q<Button>(name: _completeButtonName);
            _heroesList = Root.Q<ScrollView>(name: _heroesListName);
            _heroName = Root.Q<Label>(name: _heroNameName);
            _heroDescription = Root.Q<Label>(name: _heroDescriptionName);

            // _completeButton.RegisterCallback<ClickEvent>(PlaySelectionSFX);
        }

        protected override void OnBind(HeroSelectionViewModel viewModel)
        {
            base.OnBind(viewModel);
            ViewModel.IsLoaded.Where(x => x == true).Take(1).Subscribe(_ => OnLoadingCompletedBinding())
                .AddTo(_disposables);
        }

        protected override void OnClosing()
        {
            PlayClosingSFX();
            base.OnClosing();
        }
#endregion

        private void OnLoadingCompletedBinding()
        {
            ViewModel.AllHeroesData.Where(x => x != null).Subscribe(data =>
            {
                _heroSlotsMap = new();
                _heroesList.Clear();
                foreach (var heroData in data)
                {
                    var heroSlot = _heroSlot.CloneTree();
                    var heroLabel = heroSlot.Q<Label>(name: _heroLabelName);
                    var heroImage = heroSlot.Q<VisualElement>(name: _heroImageName);

                    heroLabel.LocalizeText(Tables.HEROES, heroData.Name);
                    heroImage.style.backgroundImage = new StyleBackground(heroData.PreviewSprite); // TODO: individual images
                    heroImage.style.unityBackgroundImageTintColor = new StyleColor(heroData.Tint); // TODO: don`t use it
                    heroSlot.RegisterCallback<ClickEvent>(_ => ViewModel.PreviewHeroSlot(heroData.Id));
                    heroSlot.RegisterCallback<PointerEnterEvent>(PlaySlotHoverSFX);

                    _heroesList.Add(heroSlot);
                    _heroSlotsMap[heroData.Id] = heroSlot.Q<VisualElement>(name: _heroSlotName);
                }
            }).AddTo(_disposables);

            ViewModel.CurrentHeroDataPreview.Subscribe(OnHeroPreviewed).AddTo(_disposables);

            _completeButton.RegisterCallbackOnce<ClickEvent>(CompleteSelection);
        }
        
        private void CompleteSelection(ClickEvent clickEvent)
        {
            ViewModel.CompleteSelection();
        }

        private void OnHeroPreviewed(IHeroData heroData)
        {
            if (heroData != null)
            {
                _selectedSlot?.RemoveFromClassList(_heroSlotSelectedClass);
                _selectedSlot = _heroSlotsMap[heroData.Id];
                _selectedSlot.AddToClassList(_heroSlotSelectedClass);
                _heroName.LocalizeText(Tables.HEROES, heroData.Name);
                _heroDescription.LocalizeText(Tables.HEROES, heroData.Description);
            }
        }

        private void PlaySlotHoverSFX(PointerEnterEvent pointerEnterEvent)
        {
            _audioPlayer.PlayOneShotSFX(_slotHoverSFX);
        }

        private void PlayClosingSFX()
        {
            _audioPlayer.PlayRandomPitchSFX(_closingSFX);
        }
    }
}