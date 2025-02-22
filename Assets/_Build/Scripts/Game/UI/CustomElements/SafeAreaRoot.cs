using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    [UxmlElement]
    public partial class SafeAreaRoot : VisualElement
    {
        [UxmlAttribute] private float _topPaddingScale = 1;
        [UxmlAttribute] private float _rightPaddingScale = 1;
        [UxmlAttribute] private float _bottomPaddingScale = 1;
        [UxmlAttribute] private float _leftPaddingScale = 1;
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

            style.paddingTop = _topBorder * _topPaddingScale;
            style.paddingRight = _rightBorder * _rightPaddingScale;
            style.paddingBottom = _bottomBorder * _bottomPaddingScale;
            style.paddingLeft = _leftBorder * _leftPaddingScale;
        }
    }
}