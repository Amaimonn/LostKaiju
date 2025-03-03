using UnityEngine;
using R3;

using LostKaiju.Game.World.Player.Views;

namespace LostKaiju.Game.World.Missions
{
    [RequireComponent(typeof(Collider2D))]
    public class MissionExitAreaTrigger : MonoBehaviour
    {
        public Observable<IPlayerHero> OnPlayerHeroEnter => _onPlayerHeroEnter;
        [SerializeField] private Collider2D _triggerCollider;
        private readonly Subject<IPlayerHero> _onPlayerHeroEnter = new();

        private void OnPlayerEnter(IPlayerHero playerHero)
        {
            _onPlayerHeroEnter.OnNext(playerHero);
        }
        
#region MonoBehaviour
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IPlayerHero>(out var playerHero))
            {
                Debug.Log("Player has entered exit area");
                OnPlayerEnter(playerHero);
            }
        }
#endregion
    }
}