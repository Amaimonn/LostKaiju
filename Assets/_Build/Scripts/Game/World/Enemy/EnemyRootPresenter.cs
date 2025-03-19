using UnityEngine;
using VContainer;

using LostKaiju.Services.Audio;

namespace LostKaiju.Game.World.Enemy
{
    public abstract class EnemyRootPresenter : MonoBehaviour
    {
        protected AudioPlayer _audioPlayer;
        
        [Inject]
        public virtual void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        public abstract void Init();
    }
}