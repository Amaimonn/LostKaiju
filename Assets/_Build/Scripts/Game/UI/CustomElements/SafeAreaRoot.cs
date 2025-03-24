using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    public class SafeAreaRoot : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SafeAreaRoot, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlFloatAttributeDescription _topPaddingScale = new UxmlFloatAttributeDescription { name = "top-padding-scale", defaultValue = 1f };
            private readonly UxmlFloatAttributeDescription _rightPaddingScale = new UxmlFloatAttributeDescription { name = "right-padding-scale", defaultValue = 1f };
            private readonly UxmlFloatAttributeDescription _bottomPaddingScale = new UxmlFloatAttributeDescription { name = "bottom-padding-scale", defaultValue = 1f };
            private readonly UxmlFloatAttributeDescription _leftPaddingScale = new UxmlFloatAttributeDescription { name = "left-padding-scale", defaultValue = 1f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var safeAreaRoot = ve as SafeAreaRoot;

                safeAreaRoot.TopPaddingScale = _topPaddingScale.GetValueFromBag(bag, cc);
                safeAreaRoot.RightPaddingScale = _rightPaddingScale.GetValueFromBag(bag, cc);
                safeAreaRoot.BottomPaddingScale = _bottomPaddingScale.GetValueFromBag(bag, cc);
                safeAreaRoot.LeftPaddingScale = _leftPaddingScale.GetValueFromBag(bag, cc);
            }
        }

        public float TopPaddingScale { get; set; } = 1f;
        public float RightPaddingScale { get; set; } = 1f;
        public float BottomPaddingScale { get; set; } = 1f;
        public float LeftPaddingScale { get; set; } = 1f;

        private float _leftBorder;
        private float _rightBorder;
        private float _topBorder;
        private float _bottomBorder;

        public SafeAreaRoot()
        {
            RegisterCallback<GeometryChangedEvent>(evt => OnSafeAreaChanged());
        }

        void OnSafeAreaChanged()
        {
            Rect safeArea = Screen.safeArea;

            _topBorder = Screen.height - safeArea.yMax;
            _rightBorder = Screen.width - safeArea.xMax;
            _bottomBorder = safeArea.y;
            _leftBorder = safeArea.x;

            style.paddingTop = _topBorder * TopPaddingScale;
            style.paddingRight = _rightBorder * RightPaddingScale;
            style.paddingBottom = _bottomBorder * BottomPaddingScale;
            style.paddingLeft = _leftBorder * LeftPaddingScale;
        }
    }
}