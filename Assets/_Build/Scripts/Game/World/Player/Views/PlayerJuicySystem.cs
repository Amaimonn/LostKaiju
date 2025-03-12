using UnityEngine;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Player.Views
{
    public class PlayerJuicySystem : MonoBehaviour, IJuicySystem
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _stepParticles;
        [SerializeField] private AudioClip _stepSound;
        [SerializeField] private AudioClip _attackSound;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private ParticleSystem _hitParticles;

        public void PlayStep()
        {
            _stepParticles.Play();
            _audioSource.PlayOneShot(_stepSound);
        }

        public void PlayAttack()
        {
            _audioSource.PlayOneShot(_attackSound);
        }

        public void PlayHit(Vector3 point)
        {
            _hitParticles.transform.position = point;
            _hitParticles.Play();
        }
    }
}