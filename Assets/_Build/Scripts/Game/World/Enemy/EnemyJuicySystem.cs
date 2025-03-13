using UnityEngine;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Enemy
{
    public class EnemyJuicySystem : MonoBehaviour, IJuicySystem
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _attackSound;
        [SerializeField] private AudioClip _deathSound;
        [SerializeField] private Effect _onDamagedEffect;
        
        public void PlayAttack()
        {
            _audioSource.PlayOneShot(_attackSound);
        }

        public void PlayOnDamaged()
        {
            _onDamagedEffect.PlayEffect();
        }
    }
}