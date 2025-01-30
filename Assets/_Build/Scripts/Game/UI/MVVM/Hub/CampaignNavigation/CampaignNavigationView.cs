using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class CampaignNavigationView : ToolkitView<CampaignNavigationViewModel>
    {
        [Header("UI Elements")]
        [SerializeField] private string _contentElementName;
        [SerializeField] private string _startButtonName;
        [SerializeField] private string _closeButtonName;
        [SerializeField] private string _selectedMissionLabelName;
        [SerializeField] private string _selectedMissionTextName;
        [SerializeField] private string _contentStyleName;
        [SerializeField] private string _missionsGridName;
        [SerializeField] private string _baseButtonStyleName;
        [SerializeField] private string _missionButtonStyleName;
        [SerializeField] private string _panelWhiteBackgroundStyleName;

        [Header("SFX")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _buttonHoverSFX;

        private Button _startButton;
        private Button _closeButton;
        private VisualElement _contentElement;
        private Label _selectedMissionLabel;
        private Label _selectedMissionText;
        private VisualElement _missionsGrid;
        private VisualElement _panelWhiteBackground;
        private bool _isGameplayStarted = false;
        private bool _isClosing = false;

        protected override void OnAwake()
        {
            _contentElement = _root.Q<VisualElement>(name: _contentElementName);
            _startButton = _root.Q<Button>(name: _startButtonName);
            _closeButton = _root.Q<Button>(name: _closeButtonName);
            _selectedMissionLabel = _root.Q<Label>(name: _selectedMissionLabelName);
            _selectedMissionText = _root.Q<Label>(name: _selectedMissionTextName);
            _missionsGrid = _root.Q<VisualElement>(name: _missionsGridName);
            _panelWhiteBackground = _root.Q<VisualElement>(className: _panelWhiteBackgroundStyleName);
            _contentElement.AddToClassList($"{_contentStyleName}--disabled");
            _contentElement.RegisterCallback<TransitionEndEvent>(_ =>
            {
                if (_isClosing)
                    _viewModel.CompleteClose();
            });
        }

        protected override void OnBind(CampaignNavigationViewModel viewModel)
        {
            _startButton.RegisterCallback<ClickEvent>(_ => StartGameplay());
            _closeButton.RegisterCallback<ClickEvent>(_ => Close());

            foreach (var mission in _viewModel.DisplayedMissionsData)
            {
                var missionButton = new Button
                {
                    text = mission.DysplayedNumber,
                };
                
                if (_viewModel.AvailableMissionsMap.TryGetValue(mission.Id, out MissionModel missionModel))
                {
                    if (missionModel.IsCompleted.Value)
                    {
                        // mark mission as completed
                    }
                }
                else
                {
                    missionButton.style.color = Color.gray;
                    // mark mission as locked
                }
                missionButton.RegisterCallback<ClickEvent>(_ => _viewModel.SelectMission(mission));
                missionButton.RegisterCallback<PointerEnterEvent>(_ => PlayButtonHoverSFX());
                missionButton.AddToClassList(_baseButtonStyleName);
                missionButton.AddToClassList(_missionButtonStyleName);
                _missionsGrid.Add(missionButton);
            }

            _viewModel.OnOpenStateChanged.Skip(1).Subscribe(e => OnOpedStateChanged(e));
            _viewModel.SelectedMission.Subscribe(OnMissionSelected);
        }

        private void StartGameplay()
        {
            if (_isGameplayStarted)
            {
                return;
            }
            _viewModel.StartGameplay();
            _isGameplayStarted = true;
        }

        private void Close()
        {
            Debug.Log("Missions: close button clicked");
            _viewModel.Close();
        }

        private void OnOpedStateChanged(bool isOpened)
        {
            if (isOpened)
                OnOpened();
            else
                OnClosed();
        }

        private void OnOpened()
        {
            StartCoroutine(OpenAnimation());
        }

        private IEnumerator OpenAnimation()
        {
            yield return null;
            _contentElement.RemoveFromClassList($"{_contentStyleName}--disabled");
            _panelWhiteBackground.AddToClassList($"{_panelWhiteBackgroundStyleName}--enabled");
            Debug.Log("Missions: opened");
        }

        private void OnClosed()
        {
            _isClosing = true;
            _contentElement.AddToClassList($"{_contentStyleName}--disabled");
            _panelWhiteBackground.RemoveFromClassList($"{_panelWhiteBackgroundStyleName}--enabled");
            Debug.Log("Missions: closed");
        }

        private void OnMissionSelected(IMissionData missionModel)
        {
            if (_viewModel.AvailableMissionsMap.ContainsKey(missionModel.Id))
            {
                _startButton.enabledSelf = true;
            }
            else
            {
                _startButton.enabledSelf = false;
            }

            _selectedMissionLabel.text = missionModel.Name;
            _selectedMissionText.text = missionModel.Text;
        }

        private void PlayButtonHoverSFX()
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_buttonHoverSFX);
        }
    }
}
