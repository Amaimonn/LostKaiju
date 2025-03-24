using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.UI.Extentions;
using LostKaiju.Game.Constants;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class CampaignNavigationView : PopUpToolkitView<CampaignNavigationViewModel>
    {
        [Header("UI Elements")]
        [SerializeField] private string _campaignTitleName;
        [SerializeField] private string _contentName;
        [SerializeField] private string _contentClass;
        [SerializeField] private string _startButtonName;
        [SerializeField] private string _locationTabsContainerName;
        [SerializeField] private string _selectedMissionLabelName;
        [SerializeField] private string _selectedMissionTextName;
        [SerializeField] private string _missionTextScrollViewName;
        [SerializeField] private string _missionsGridName;
        [SerializeField] private string _panelWhiteBackgroundClass;

        [Header("SFX")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _buttonHoverSFX;

        [Space(3f)]
        [Header("Assets")]
        [SerializeField] private VisualTreeAsset _missionButton;
        [SerializeField] private string _missionButtonSelectedClass;
        [SerializeField] private string _missionButtonCompletedClass;

        [Space(2f)]
        [SerializeField] private VisualTreeAsset _locationTabButton;
        [SerializeField] private string _locationButtonSelectedClass;

        private AudioPlayer _audioPlayer;
        private Button _startButton;
        private VisualElement _content;
        private VisualElement _locationTabsContainer;
        private Label _selectedMissionLabel;
        private Label _selectedMissionText;
        private ScrollView _missionTextScrollView;
        private VisualElement _missionsGrid;
        private VisualElement _panelWhiteBackground;
        private bool _isClosing = false;
        private readonly Dictionary<string, Button> _missionButtonsMap = new();
        private readonly Dictionary<string, VisualElement> _locationTabButtonsMap = new();
        private Button _selectedMissionButton;
        private VisualElement _selectedLocationTab;
        private readonly SerialDisposable _missionTextDisposable = new();
        private readonly SerialDisposable _missionLabelDisposable = new();

        public void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _content = Root.Q<VisualElement>(name: _contentName);
            _startButton = Root.Q<Button>(name: _startButtonName);
            _locationTabsContainer = Root.Q<VisualElement>(name: _locationTabsContainerName);
            _selectedMissionLabel = Root.Q<Label>(name: _selectedMissionLabelName);
            _selectedMissionText = Root.Q<Label>(name: _selectedMissionTextName);
            _missionTextScrollView = Root.Q<ScrollView>(name: _missionTextScrollViewName);
            _missionsGrid = Root.Q<VisualElement>(name: _missionsGridName);
            _panelWhiteBackground = Root.Q<VisualElement>(className: _panelWhiteBackgroundClass);

            Root.Q<Label>(name: _campaignTitleName).LocalizeText(Tables.UI, "missions")
                .AddTo(_disposables);
            _startButton.LocalizeText(Tables.UI, "start").AddTo(_disposables);

            _content.AddToClassList($"{_contentClass}--disabled");
            _content.RegisterCallback<TransitionEndEvent>(_ =>
            {
                if (_isClosing)
                    ViewModel.CompleteClosing();
            });
            
        }
        
        protected override void OnBind(CampaignNavigationViewModel viewModel)
        {
            base.OnBind(viewModel);
            ViewModel.IsLoaded.Where(x => x == true).Take(1).Subscribe(_ => OnLoadingCompletedBinding()).AddTo(_disposables);
        }

        private void OnLoadingCompletedBinding()
        {
            _startButton.RegisterCallback<ClickEvent>(StartGameplay);

            BindDisplayedLocationsButtons();
            
            ViewModel.DisplayedMissionsData.Subscribe(BindDisplayedMissionsButtons).AddTo(_disposables);
            ViewModel.SelectedLocation.Subscribe(OnLocationSelected).AddTo(_disposables);
            ViewModel.SelectedMission.Subscribe(OnMissionSelected).AddTo(_disposables);
        }

        private void BindDisplayedLocationsButtons()
        {
            _locationTabsContainer.Clear();
            _locationTabButtonsMap.Clear();

            var selectedLocationId = ViewModel.SelectedLocation.CurrentValue.Id;
            foreach (var locationData in ViewModel.DisplayedLocationsData)
            {
                var locationTabButtonContainer = _locationTabButton.CloneTree();
                var locationTabButton = locationTabButtonContainer.Q<Button>();
                var locationLabel = locationTabButtonContainer.Q<Label>();
                locationLabel.LocalizeText(Tables.CAMPAIGN, locationData.Name).AddTo(_disposables);

                if (ViewModel.AvailableLocationsMap.TryGetValue(locationData.Id, out var locationModel))
                {
                    // if (locationModel.IsCompleted.Value)
                    // {
                    //     // mark location as completed
                    // }
                    locationTabButton.RegisterCallback<ClickEvent>(_ => ViewModel.SelectLocation(locationData));
                    // locationTabButton.RegisterCallback<PointerEnterEvent>(PlayButtonHoverSFX);
                    _locationTabButtonsMap[locationData.Id] = locationTabButton;
                }
                else
                {
                    locationTabButton.SetEnabled(false);
                }

                _locationTabsContainer.Add(locationTabButtonContainer);
            }
        }
        private void BindDisplayedMissionsButtons(IMissionData[] missions)
        {
            _missionsGrid.Clear();
            _missionButtonsMap.Clear();
            
            foreach (var missionData in missions)
            {
                var missionButtonContainer = _missionButton.CloneTree();
                var missionButton = missionButtonContainer.Q<Button>();
                missionButton.text = missionData.DysplayedNumber;

                if (ViewModel.AvailableMissionsMap.TryGetValue(missionData.Id, out var missionModel))
                {
                    if (missionModel.IsCompleted.Value)
                    {
                        missionButton.AddToClassList(_missionButtonCompletedClass);
                    }
                    _missionButtonsMap[missionData.Id] = missionButton;
                    missionButton.RegisterCallback<PointerEnterEvent>(PlayButtonHoverSFX);
                    missionButton.RegisterCallback<ClickEvent>(_ => ViewModel.SelectMission(missionData));
                }
                else
                {
                    missionButton.SetEnabled(false);
                }
                _missionsGrid.Add(missionButtonContainer);
            }
        }

        private void StartGameplay(ClickEvent clickEvent)
        {
            _startButton.RegisterCallback<ClickEvent>(StartGameplay);
            ViewModel.StartGameplay();
        }

#region PopUpToolkitView
        protected override void StartClosing(ClickEvent clickEvent)
        {
            Debug.Log("Missions: close button clicked");
            base.StartClosing(clickEvent);
        }

        protected override void OnOpening()
        {
            StartCoroutine(OpenAnimation());

            IEnumerator OpenAnimation()
            {
                yield return null;
                _content.RemoveFromClassList($"{_contentClass}--disabled");
                _panelWhiteBackground.AddToClassList($"{_panelWhiteBackgroundClass}--enabled");
                Debug.Log("Missions: opened");
            }
        }

        protected override void OnClosing()
        {
            _isClosing = true;
            _content.AddToClassList($"{_contentClass}--disabled");
            _panelWhiteBackground.RemoveFromClassList($"{_panelWhiteBackgroundClass}--enabled");
            Debug.Log("Missions: closed");
        }
#endregion

        private void OnLocationSelected(ILocationData locationData)
        {
            if (locationData != null)
            {
                _selectedLocationTab?.RemoveFromClassList(_locationButtonSelectedClass);
                _locationTabButtonsMap[locationData.Id].AddToClassList(_locationButtonSelectedClass);
                _selectedLocationTab = _locationTabButtonsMap[locationData.Id];
            }
        }

        private void OnMissionSelected(IMissionData missionData)
        {
            if (missionData != null)
            {
                _selectedMissionButton?.RemoveFromClassList(_missionButtonSelectedClass);

                if (ViewModel.AvailableMissionsMap.ContainsKey(missionData.Id))
                    _startButton.SetEnabled(true);
                else
                    _startButton.SetEnabled(false);

                if (_missionButtonsMap.TryGetValue(missionData.Id, out var button))
                {
                    button.AddToClassList(_missionButtonSelectedClass);
                    _selectedMissionButton = button;
                }
                else
                {
                    Debug.LogWarning($"No button for {missionData.Id} missionId");
                }

                _missionLabelDisposable.Disposable = _selectedMissionLabel.LocalizeText(Tables.CAMPAIGN, missionData.Name);
                _missionTextDisposable.Disposable = _selectedMissionText.LocalizeText(Tables.CAMPAIGN, missionData.Text);
            }
            else
            {
                _startButton.SetEnabled(false);
                _selectedMissionLabel.text = string.Empty;
                _selectedMissionText.text = string.Empty;
            }
            _missionTextScrollView.scrollOffset = Vector2.zero;
        }

        private void PlayButtonHoverSFX(PointerEnterEvent pointerEnterEvent)
        {
            // var randomPitch = Random.Range(0.9f, 1.1f);
            _audioPlayer.PlayOneShotSFX(_buttonHoverSFX);
        }

        public override void Dispose()
        {
            _missionTextDisposable?.Dispose();
            _missionLabelDisposable?.Dispose();
            base.Dispose();
        }
    }
}
