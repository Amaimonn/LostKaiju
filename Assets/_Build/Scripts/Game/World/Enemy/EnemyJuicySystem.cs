using UnityEngine;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.VFX;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.World.Enemy
{
    public class EnemyJuicySystem : MonoBehaviour, IJuicySystem
    {
        [SerializeField] private AudioClip _attackSound;
        [SerializeField] private AudioClip _deathSound;
        [SerializeField] private Effect _onDamagedEffect;
        private AudioPlayer _audioPlayer;
        
        public void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }
        
        public void PlayAttack()
        {
            if (_attackSound != null)
                _audioPlayer.PlayOneShotSFX(_attackSound);
        }

        public void PlayOnDamaged()
        {
            if (_onDamagedEffect != null)
                _onDamagedEffect.PlayEffect();
        }
    }
}