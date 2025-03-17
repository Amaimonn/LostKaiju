using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HeroSelectionView : ToolkitView<HeroSelectionViewModel>
    {
        [SerializeField] private string _closeButtonName;
        [SerializeField] private string _completeButtonName;

        private Button _closeButton;
        private Button _completeButton;
        private bool _isStartPressed = false;

        protected override void OnAwake()
        {
            _closeButton = Root.Q<Button>(name: _closeButtonName);
            _completeButton = Root.Q<Button>(name: _completeButtonName);
        }

        protected override void OnBind(HeroSelectionViewModel viewModel)
        {
            _completeButton.RegisterCallbackOnce<ClickEvent>(CompleteSelection);
            _closeButton.RegisterCallbackOnce<ClickEvent>(Close);
            
            ViewModel.OnOpenStateChanged.Skip(1).Subscribe(OnOpenStateChanged);
            ViewModel.IsLoaded.Where(x => x == true).Take(1).Subscribe(_ => OnLoadingCompletedBinding());
        }

        private void OnLoadingCompletedBinding()
        {

        }
        
        private void CompleteSelection(ClickEvent clickEvent)
        {
            if (_isStartPressed)
            {
                return;
            }
            ViewModel.CompleteSelection();
            _isStartPressed = true;
        }

        private void Close(ClickEvent clickEvent)
        {
            Debug.Log("HeroSelection: close button clicked");
            ViewModel.StartClosing();
        }

        private void OnOpenStateChanged(bool isOpened)
        {
            if (isOpened)
                OnOpened();
            else
                OnClosed();

            void OnOpened()
            {
                // StartCoroutine(OpenAnimation());

                // IEnumerator OpenAnimation()
                // {
                //     yield return null;
                //     _contentElement.RemoveFromClassList($"{_contentStyleName}--disabled");
                //     _panelWhiteBackground.AddToClassList($"{_panelWhiteBackgroundStyleName}--enabled");
                // }
            }

            void OnClosed()
            {
                ViewModel.CompleteClosing();
                // _isClosing = true;
                // _contentElement.AddToClassList($"{_contentStyleName}--disabled");
                // _panelWhiteBackground.RemoveFromClassList($"{_panelWhiteBackgroundStyleName}--enabled");
            }
        }
    }
}