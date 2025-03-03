using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Missions.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TargetTrigger<T> : MonoBehaviour
    {
        public Observable<T> OnEnter => _onEnter;
        public Observable<T> OnExit => _onExit;
        [SerializeField] protected Collider2D _triggerCollider;
        protected readonly Subject<T> _onEnter = new();
        protected readonly Subject<T> _onExit = new();

#region MonoBehaviour
        protected virtual void Awake()
        {
            _triggerCollider.isTrigger = true;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var target))
            {
                OnTargetEnter(target);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var target))
            {
                OnTargetExit(target);
            }
        }
#endregion

        protected virtual void OnTargetEnter(T target)
        {
            _onEnter.OnNext(target);
        }

        protected virtual void OnTargetExit(T target)
        {
            _onExit.OnNext(target);
        }
    }
}