using UnityEngine;

namespace LostKaiju.Utils
{
    public class Timer
    {
        public bool IsCompleted { get; private set; }
        
        private float _startTime;
        private float _currentTime;
        private float _waitTime;

        public Timer(float waitTime, bool isCompleted = false)
        {
            if (waitTime < 0)
            {
                Debug.LogWarning("Timer's wait time < 0. This is strange");
            }
            
            if (isCompleted)
            {
                IsCompleted = true;
            }
            else
            {
                _currentTime = Time.time;
                _startTime = Time.time;
            }

            _waitTime = waitTime;
        }

        public void Tick()
        {
            if (IsCompleted)
                return;

            if (!CheckTimeIsOver())
                _currentTime = Time.time;
            else
                IsCompleted = true;
        }

        public void Refresh()
        {
            _currentTime = Time.time;
            _startTime = Time.time;
            IsCompleted = CheckTimeIsOver();
        }

        private bool CheckTimeIsOver()
        {
            return _currentTime >= _startTime + _waitTime;
        }
    }
}
