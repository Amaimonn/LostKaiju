using System;
using UnityEngine;
using R3;

namespace LostKaiju.Utils
{
    [Serializable]
    public class ClampedValue<T> where T : IComparable
    {
        public T CurrentValue => _currentValue;
        public Observable<T> OnChanged => _onChanged;
        public bool IsFull => _currentValue.Equals(_topLimit);
        public bool IsEmpty() => _currentValue.Equals(_bottomLimit);

        [SerializeField] protected T _currentValue;
        [SerializeField] protected T _bottomLimit;
        [SerializeField] protected T _topLimit;

        protected Subject<T> _onChanged = new();

        public ClampedValue(T bottomLimit, T topLimit, T initialValue)
        {
            _bottomLimit = bottomLimit;
            _topLimit = topLimit;
            SetValue(initialValue);
        }

        public void SetValue(T value)
        {
            if (_topLimit.CompareTo(value) < 0)
                _currentValue = _topLimit;
            else if (_bottomLimit.CompareTo(value) > 0)
                _currentValue = _bottomLimit;
            else
                _currentValue = value;

            _onChanged.OnNext(_currentValue);
        }

        public void Refresh()
        {
            SetValue(_topLimit);
        }
    }
}