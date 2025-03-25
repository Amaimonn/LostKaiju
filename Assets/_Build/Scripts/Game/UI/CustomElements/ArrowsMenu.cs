using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    [UxmlElement]
    public partial class ArrowsMenu : BaseField<string>
    {
        // USS class names
        public new const string ussClassName = "arrows-menu";
        public const string valueLabelUssClassName = "arrows-menu__value-label";
        public const string arrowButtonUssClassName = "arrows-menu__arrow-button";
        public const string leftArrowButtonUssClassName = "arrows-menu__arrow-button--left";
        public const string rightArrowButtonUssClassName = "arrows-menu__arrow-button--right";

        private string[] _options = Array.Empty<string>();
        private int _currentIndex = 0;
        private string _leftButtonText = "<";
        private string _rightButtonText = ">";

        private readonly VisualElement _input;
        private readonly Label _valueLabel;
        private readonly Button _leftButton;
        private readonly Button _rightButton;

        [UxmlAttribute("options")]
        public string[] Options
        {
            get => _options;
            set
            {
                _options = value ?? Array.Empty<string>();
                UpdateOptions();
            }
        }

        [UxmlAttribute("left-button-text")]
        public string LeftButtonText
        {
            get => _leftButtonText;
            set
            {
                _leftButtonText = value;
                if (_leftButton != null)
                    _leftButton.text = value;
            }
        }

        [UxmlAttribute("right-button-text")]
        public string RightButtonText
        {
            get => _rightButtonText;
            set
            {
                _rightButtonText = value;
                if (_rightButton != null)
                    _rightButton.text = value;
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

        public ArrowsMenu() : this(null) { }

        public ArrowsMenu(string label) : base(label, null)
        {
            _input = this.Q(className: BaseField<bool>.inputUssClassName);
            AddToClassList(ussClassName);
            
            _leftButton = new Button() { name = "left-button", text = LeftButtonText };
            _leftButton.AddToClassList(arrowButtonUssClassName);
            _leftButton.AddToClassList(leftArrowButtonUssClassName);
            _leftButton.clicked += () => OnArrowButtonClicked(-1);
            
            _valueLabel = new Label() { name = "value-label"};
            _valueLabel.AddToClassList(valueLabelUssClassName);
            
            _rightButton = new Button() { name = "right-button", text = RightButtonText };
            _rightButton.AddToClassList(arrowButtonUssClassName);
            _rightButton.AddToClassList(rightArrowButtonUssClassName);
            _rightButton.clicked += () => OnArrowButtonClicked(1);

            _input.Add(_leftButton);
            _input.Add(_valueLabel);
            _input.Add(_rightButton);

            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.Center;

            UpdateValueLabel();
        }

        private void OnArrowButtonClicked(int direction)
        {
            if (Options != null && Options.Length > 0)
                CurrentIndex = (CurrentIndex + direction + Options.Length) % Options.Length;
        }

        private void UpdateValueLabel()
        {
            if (_valueLabel == null)
                return;
                
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