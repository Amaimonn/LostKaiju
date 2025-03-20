using UnityEngine;
using VContainer;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.VFX;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.World.Player.Views
{
    public class PlayerJuicySystem : MonoBehaviour, IJuicySystem
    {
        [SerializeField] private ParticleSystem _stepParticles;
        [SerializeField] private AudioClip _stepSound;
        [SerializeField] private AudioClip _attackSound;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private Effect _onDamagedEffect;
        [SerializeField] private AudioClip _onDamagedSound;
        
        [Inject] private AudioPlayer _audioPlayer;

        public void PlayStep()
        {
            _stepParticles.Play();
            _audioPlayer.PlayOneShotSFX(_stepSound);
        }

        public void PlayAttack()
        {
            _audioPlayer.PlayOneShotSFX(_attackSound);
        }

        public void PlayHit(Vector3 point)
        {
            _audioPlayer.PlaySFX(_hitSound, pitch: Random.Range(0.8f, 1.2f));
            _hitParticles.transform.position = point;
            _hitParticles.Play();
        }

        public void PlayOnDamaged()
        {
            _onDamagedEffect.PlayEffect();
            _audioPlayer.PlayOneShotSFX(_onDamagedSound);
        }
    }
}