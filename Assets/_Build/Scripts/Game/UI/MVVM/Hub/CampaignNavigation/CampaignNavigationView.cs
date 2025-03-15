using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.UI.MVVM.Gameplay;
using UnityEngine.Localization;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class CampaignNavigationView : PopUpToolkitView<CampaignNavigationViewModel>
    {
        [Header("UI Elements")]
        [SerializeField] private string _contentElementName;
        [SerializeField] private string _startButtonName;
        [SerializeField] private string _locationTabsContainerName;
        [SerializeField] private string _selectedMissionLabelName;
        [SerializeField] private string _selectedMissionTextName;
        [SerializeField] private string _contentStyleName;
        [SerializeField] private string _missionTextScrollViewName;
        [SerializeField] private string _missionsGridName;
        [SerializeField] private string _baseButtonStyleName;
        [SerializeField] private string _missionButtonStyleName;
        [SerializeField] private string _panelWhiteBackgroundStyleName;

        [Header("SFX")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _buttonHoverSFX;

        [Space(3f)]
        [Header("Assets")]
        [SerializeField] private VisualTreeAsset _selectMissionButton;
        [SerializeField] private string _selectMissionButtonSelectedStyleName;
        [SerializeField] private string _selectMissionButtonCompletedStyleName;

        [Space(2f)]
        [SerializeField] private VisualTreeAsset _locationTabButton;
        [SerializeField] private string _locationButtonSelectedStyleName;

        private Button _startButton;
        private VisualElement _content;
        private VisualElement _locationTabsContainer;
        private Label _selectedMissionLabel;
        private Label _selectedMissionText;
        private ScrollView _missionTextScrollView;
        private VisualElement _missionsGrid;
        private VisualElement _panelWhiteBackground;
        private bool _isGameplayStarted = false;
        private bool _isClosing = false;
        private Dictionary<string, Button> _missionButtonsMap;
        private Dictionary<string, VisualElement> _locationTabButtonsMap;
        private Button _selectedMissionButton;
        private VisualElement _selectedLocationTab;

        protected override void OnAwake()
        {
            base.OnAwake();
            _content = Root.Q<VisualElement>(name: _contentElementName);
            _startButton = Root.Q<Button>(name: _startButtonName);
            _locationTabsContainer = Root.Q<VisualElement>(name: _locationTabsContainerName);
            _selectedMissionLabel = Root.Q<Label>(name: _selectedMissionLabelName);
            _selectedMissionText = Root.Q<Label>(name: _selectedMissionTextName);
            _missionTextScrollView = Root.Q<ScrollView>(name: _missionTextScrollViewName);
            _missionsGrid = Root.Q<VisualElement>(name: _missionsGridName);
            _panelWhiteBackground = Root.Q<VisualElement>(className: _panelWhiteBackgroundStyleName);

            _content.AddToClassList($"{_contentStyleName}--disabled");
            _content.RegisterCallback<TransitionEndEvent>(_ =>
            {
                if (_isClosing)
                    ViewModel.CompleteClosing();
            });
        }
        
        protected override void OnBind(CampaignNavigationViewModel viewModel)
        {
            base.OnBind(viewModel);
            ViewModel.IsLoaded.Subscribe(isLoaded => {
                if (isLoaded)
                    OnLoadingCompletedBinding();
                else 
                {
                    // loading circle animation
                }
            });
        }

        private void OnLoadingCompletedBinding()
        {
            _startButton.RegisterCallbackOnce<ClickEvent>(StartGameplay);

            BindDisplayedLocationsButtons();
            
            ViewModel.DisplayedMissionsData.Subscribe(BindDisplayedMissionsButtons);
            ViewModel.SelectedLocation.Subscribe(OnLocationSelected);
            ViewModel.SelectedMission.Subscribe(OnMissionSelected);
        }

        private void BindDisplayedLocationsButtons()
        {
            _locationTabsContainer.Clear();
            _locationTabButtonsMap = new();
            var selectedLocationId = ViewModel.SelectedLocation.CurrentValue.Id;
            foreach (var location in ViewModel.DisplayedLocationsData)
            {
                var locationTabButtonContainer = _locationTabButton.CloneTree();
                var locationTabButton = locationTabButtonContainer.Q<Button>();
                var locationLabel = locationTabButtonContainer.Q<Label>();
                locationLabel.text = location.Name;

                if (ViewModel.AvailableLocationsMap.TryGetValue(location.Id, out var locationModel))
                {
                    if (locationModel.IsCompleted.Value)
                    {
                        // mark location as completed
                    }
                    locationTabButton.RegisterCallback<ClickEvent>(_ => ViewModel.SelectLocation(location));
                    locationTabButton.RegisterCallback<PointerEnterEvent>(PlayButtonHoverSFX);
                    _locationTabButtonsMap[location.Id] = locationTabButton;
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
            _missionButtonsMap = new();
            foreach (var mission in missions)
            {
                var missionButtonContainer = _selectMissionButton.CloneTree();
                var missionButton = missionButtonContainer.Q<Button>();
                missionButton.text = mission.DysplayedNumber;

                if (ViewModel.AvailableMissionsMap.TryGetValue(mission.Id, out var missionModel))
                {
                    if (missionModel.IsCompleted.Value)
                    {
                        missionButton.AddToClassList(_selectMissionButtonCompletedStyleName);
                    }
                    _missionButtonsMap[mission.Id] = missionButton;
                }
                else
                {
                    missionButton.SetEnabled(false);
                    // mark mission as locked
                }
                missionButton.RegisterCallback<ClickEvent>(_ => ViewModel.SelectMission(mission));
                missionButton.RegisterCallback<PointerEnterEvent>(PlayButtonHoverSFX);
                missionButton.AddToClassList(_baseButtonStyleName);
                missionButton.AddToClassList(_missionButtonStyleName);
                _missionsGrid.Add(missionButtonContainer);
            }
        }

        private void StartGameplay(ClickEvent clickEvent)
        {
            if (_isGameplayStarted)
            {
                return;
            }
            ViewModel.StartGameplay();
            _isGameplayStarted = true;
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
                _content.RemoveFromClassList($"{_contentStyleName}--disabled");
                _panelWhiteBackground.AddToClassList($"{_panelWhiteBackgroundStyleName}--enabled");
                Debug.Log("Missions: opened");
            }
        }

        protected override void OnClosing()
        {
            _isClosing = true;
            _content.AddToClassList($"{_contentStyleName}--disabled");
            _panelWhiteBackground.RemoveFromClassList($"{_panelWhiteBackgroundStyleName}--enabled");
            Debug.Log("Missions: closed");
        }
#endregion

        private void OnLocationSelected(ILocationData locationData)
        {
            if (locationData != null)
            {
                _selectedLocationTab?.RemoveFromClassList(_locationButtonSelectedStyleName);
                _locationTabButtonsMap[locationData.Id].AddToClassList(_locationButtonSelectedStyleName);
                _selectedLocationTab = _locationTabButtonsMap[locationData.Id];
            }
        }

        private void OnMissionSelected(IMissionData missionData)
        {
            if (missionData != null)
            {
                _selectedMissionButton?.RemoveFromClassList(_selectMissionButtonSelectedStyleName);

                if (ViewModel.AvailableMissionsMap.ContainsKey(missionData.Id))
                {
                    _startButton.enabledSelf = true;
                }
                else
                {
                    _startButton.enabledSelf = false;
                }

                if (_missionButtonsMap.TryGetValue(missionData.Id, out var button))
                {
                    button.AddToClassList(_selectMissionButtonSelectedStyleName);
                    _selectedMissionButton = button;
                }
                else
                {
                    Debug.LogWarning($"No button for {missionData.Id} missionId");
                }

                _selectedMissionLabel.text = missionData.Name;
                var localizedString = new LocalizedString("All", missionData.Text);
                _selectedMissionText.SetBinding("text", localizedString);
                // _selectedMissionText.text = missionData.Text;
            }
            else
            {
                _startButton.enabledSelf = false;
                _selectedMissionLabel.text = string.Empty;
                _selectedMissionText.text = string.Empty;
            }
            _missionTextScrollView.scrollOffset = Vector2.zero;
        }

        private void PlayButtonHoverSFX(PointerEnterEvent pointerEnterEvent)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_buttonHoverSFX);
        }
    }
}
