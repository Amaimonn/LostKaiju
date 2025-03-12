using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Creatures.Features
{
    public abstract class Pusher : MonoBehaviour, IPusher
    {
        public Observable<Unit> OnPushed => _onPushed;
        [SerializeField] protected Transform _ownOrigin;
        [SerializeField] protected Rigidbody2D _rigidbody;
        protected readonly Subject<Unit> _onPushed = new();

        public abstract void Push(Vector2 origin, float force);

        private void Awake()
        {
            if (_ownOrigin == null)
                _ownOrigin = transform;
        }
    }
}