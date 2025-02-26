using UnityEngine;

using LostKaiju.Game.Creatures.Features;

namespace LostKaiju.Game.Player.Views
{
    public class PlayerJuicySystem : MonoBehaviour, IJuicySystem
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _stepParticles;
        [SerializeField] private AudioClip _stepSound;
        [SerializeField] private AudioClip _attackSound;

        public void PlayStep()
        {
            _stepParticles.Play();
            _audioSource.PlayOneShot(_stepSound);
        }

        public void PlayAttack()
        {
            _audioSource.PlayOneShot(_attackSound);
        }
    }
}