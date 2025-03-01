using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public class VisualEffectsPlayer : MonoBehaviour, ICreatureFeature
    {
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private ParticleSystem _damagedParticles;
        [SerializeField] private ParticleSystem _walkParticles;

        public void PlayWalk()
        {
            _walkParticles.Play();
        }

        public void PlayHit()
        {
            _hitParticles.Play();
        }

        public void PlayDamaged()
        {
            _damagedParticles.Play();
        }
    }
}    
