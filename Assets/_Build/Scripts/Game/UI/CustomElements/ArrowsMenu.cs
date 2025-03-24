using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    // [UxmlElement]
    public partial class ArrowsMenu : VisualElement
    {
        private string[] _options = Array.Empty<string>();

        // [UxmlAttribute("options")]
        public string[] Options
        {
            get => _options;
            set
            {
                _options = value ?? Array.Empty<string>();
                UpdateOptions();
            }
        }

        public event Action<int> OnValueChanged;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (_currentIndex != value)
                {
                    if (Options != null && Options.Length > 0)
                    {
                        _currentIndex = Mathf.Clamp(value, 0, Options.Length - 1);
                        OnValueChanged?.Invoke(_currentIndex);
                        UpdateValueLabel();
                    }
                    else
                    {
                        _currentIndex = 0;
                        UpdateValueLabel();
                    }
                }
            }
        }

        private readonly Label _valueLabel;
        private readonly Button _leftButton;
        private readonly Button _rightButton;
        private int _currentIndex = 0;

        public ArrowsMenu()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.SpaceBetween;
            style.width = 300;

            _leftButton = new Button(() => OnArrowButtonClicked(-1))
            {
                text = "<",
                name = "left-arrow",
                style =
                {
                    width = 30,
                    height = 30,
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f),
                    color = Color.white,
                    borderTopWidth = 0,
                    borderRightWidth = 0,
                    borderBottomWidth = 0,
                    borderLeftWidth = 0,
                    borderTopLeftRadius = 5,
                    borderTopRightRadius = 5,
                    borderBottomRightRadius = 5,
                    borderBottomLeftRadius = 5
                }
            };
            Add(_leftButton);

            _valueLabel = new Label { name = "setting-value", style = { fontSize = 16, color = Color.white, marginLeft = 10, marginRight = 10 } };
            Add(_valueLabel);

            _rightButton = new Button(() => OnArrowButtonClicked(1))
            {
                text = ">",
                name = "right-arrow",
                style =
                {
                    width = 30,
                    height = 30,
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f),
                    color = Color.white,
                    borderTopWidth = 0,
                    borderRightWidth = 0,
                    borderBottomWidth = 0,
                    borderLeftWidth = 0,
                    borderTopLeftRadius = 5,
                    borderTopRightRadius = 5,
                    borderBottomRightRadius = 5,
                    borderBottomLeftRadius = 5
                }
            };
            Add(_rightButton);

            UpdateValueLabel();
        }

        private void OnArrowButtonClicked(int direction)
        {
            if (Options != null && Options.Length > 0)
                CurrentIndex = (CurrentIndex + direction + Options.Length) % Options.Length;
        }

        private void UpdateValueLabel()
        {
            if (Options != null && Options.Length > 0 && CurrentIndex >= 0 && CurrentIndex < Options.Length)
                _valueLabel.text = Options[CurrentIndex];
            else
                _valueLabel.text = string.Empty;
        }

        private void UpdateOptions()
        {
            _currentIndex = 0;
            UpdateValueLabel();
        }
    }
}