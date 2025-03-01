using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Missions
{
    public class MissionExitAreaTrigger : MonoBehaviour
    {
        public Observable<Unit> OnTriggerEnter => _onTriggerEnter;
        [SerializeField] private Collider2D _triggerCollider;
        private readonly Subject<Unit> _onTriggerEnter = new();

        private void OnPlayerEnter()
        {
            _onTriggerEnter.OnNext(Unit.Default);
        }

        private void TriggerEnter2D(Collider2D collision)
        {
        }
    }
}