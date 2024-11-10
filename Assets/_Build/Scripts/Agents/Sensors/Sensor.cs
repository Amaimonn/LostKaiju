using UnityEngine;
using R3;

namespace Assets._Build.Scripts.Agents.Sensors
{
    public abstract class Sensor<T> : MonoBehaviour
    {
        public Observable<T> Detected => _detected;
        protected ReactiveProperty<T> _detected = new();

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var target))
            {
                _detected.Value = target;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var target))
            {
                if (target.Equals(_detected.Value))
                    _detected.Value = default;
            }
        }
    }
}
