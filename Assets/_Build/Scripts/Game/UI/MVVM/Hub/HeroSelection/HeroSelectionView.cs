using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.Constants;
using LostKaiju.Game.UI.Extentions;
using LostKaiju.Game.GameData.Heroes;

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
        [SerializeField] private string _heroSlotSelectedClass;

        // [Space(4)]
        // [SerializeField] private RenderTexture _heroRenderTexture;

        private Button _completeButton;
        private ScrollView _heroesList;
        private Label _heroName;
        private Label _heroDescription;
        private VisualElement _selectedSlot;
        private Dictionary<string, VisualElement> _heroSlotsMap;

        protected override void OnAwake()
        {
            base.OnAwake();
            _completeButton = Root.Q<Button>(name: _completeButtonName);
            _heroesList = Root.Q<ScrollView>(name: _heroesListName);
            _heroName = Root.Q<Label>(name: _heroNameName);
            _heroDescription = Root.Q<Label>(name: _heroDescriptionName);
        }

        protected override void OnBind(HeroSelectionViewModel viewModel)
        {
            base.OnBind(viewModel);
            ViewModel.IsLoaded.Where(x => x == true).Take(1).Subscribe(_ => OnLoadingCompletedBinding()).AddTo(_disposables);
        }

        public void SetHeroRenderTexture(RenderTexture heroRenderTexture)
        {
            var selectedImage = Root.Q<VisualElement>(name: _selectedImageName);
            selectedImage.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(heroRenderTexture));
        }

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
                    heroLabel.LocalizeText(Tables.HEROES, heroData.Name);
                    heroSlot.RegisterCallback<ClickEvent>(_ => ViewModel.PreviewHeroSlot(heroData.Id));
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
    }
}