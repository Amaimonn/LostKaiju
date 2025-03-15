using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public abstract class PopUpToolkitView<T> : ScreenToolkitView<T> where T : ScreenViewModel
    {
        [SerializeField] protected string _closeButtonName;
        [SerializeField] protected string _closeBackgroundName;
        protected Button _closeButton;
        protected VisualElement _closeBackground;

        protected override void OnAwake()
        {
            base.OnAwake();

            if (!String.IsNullOrEmpty(_closeButtonName))
                _closeButton = Root.Q<Button>(name: _closeButtonName);

            if (!String.IsNullOrEmpty(_closeBackgroundName))
                _closeBackground = Root.Q<VisualElement>(name: _closeBackgroundName);
        }

        protected override void OnBind(T viewModel)
        {
            base.OnBind(viewModel);

            _closeButton?.RegisterCallbackOnce<ClickEvent>(StartClosing);
            _closeBackground?.RegisterCallbackOnce<ClickEvent>(StartClosing);
        }

        protected virtual void StartClosing(ClickEvent clickEvent)
        {
            ViewModel.StartClosing();
        }
    }
}