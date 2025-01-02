using System;
using UnityEngine;
using R3;

namespace LostKaiju.Utils
{
    [Serializable]
    public class ClampedValue<T> where T : IComparable
    {
        public T CurrentValue => _currentValue;
        public Observable<T> OnValueSet => _onValueSet;
        public bool IsFull => _currentValue.Equals(_maxValue);
        public bool IsEmpty => _currentValue.Equals(_minValue);

        [SerializeField] protected T _currentValue;
        [SerializeField] protected T _minValue;
        [SerializeField] protected T _maxValue;

        protected Subject<T> _onValueSet = new();

        public ClampedValue(T minValue, T maxValue, T initialValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            SetValue(initialValue);
        }

        public virtual void SetValue(T value)
        {
            if (_maxValue.CompareTo(value) < 0)
                _currentValue = _maxValue;
            else if (_minValue.CompareTo(value) > 0)
                _currentValue = _minValue;
            else
                _currentValue = value;

            _onValueSet.OnNext(_currentValue);
        }

        public void SetValueToMax()
        {
            SetValue(_maxValue);
        }

        public void SetValueToMin()
        {
            SetValue(_minValue);
        }

    }
}