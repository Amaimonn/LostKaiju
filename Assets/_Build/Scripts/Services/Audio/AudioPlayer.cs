using UnityEngine;
using R3;

using LostKaiju.Utils;

namespace LostKaiju.Services.Audio
{
    public class AudioPlayer
    {
        private readonly AudioSource _musicSource;
        private readonly AudioSource _oneShotSfxSource;
        private readonly NotNullPool<AudioSource> _sfxSourcePool;

        public AudioPlayer(Observable<float> musicVolume, Observable<float> sfxVolume)
        {
            var audioPlayerObject = new GameObject("AudioPlayer");
            _musicSource = audioPlayerObject.AddComponent<AudioSource>();
            _oneShotSfxSource = audioPlayerObject.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(audioPlayerObject);

            _sfxSourcePool = new NotNullPool<AudioSource>
            (
                createFunc: () => 
                {
                    var sfxObject = new GameObject("AudioSourceSFX");
                    var audioSource = sfxObject.AddComponent<AudioSource>();

                    audioSource.playOnAwake = false;
                    sfxVolume.TakeUntil(_ => audioSource != null)
                        .Subscribe(x => audioSource.volume = x);

                    return audioSource;
                },
                onGet: x => x.gameObject.SetActive(true),
                onRelease: x => x.gameObject.SetActive(false),
                onClear: x => Object.Destroy(x.gameObject),
                checkNotNull: x => x != null && x.gameObject != null
            );

            musicVolume.Subscribe(x => _musicSource.volume = x);
            sfxVolume.Subscribe(x => _oneShotSfxSource.volume = x);
        }
        
        public void PlayMusic(AudioClip musicClip, bool loop = true)
        {
            _musicSource.clip = musicClip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }

        /// <summary>
        /// SFX will continue playing even if scene is unloaded until it`s source stopped by script.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volumeScale"></param>
        public void PlayOneShotSFX(AudioClip clip, float volumeScale = 1.0f)
        {
            _oneShotSfxSource.PlayOneShot(clip, volumeScale);
        }

        /// <summary>
        /// SFX will stop playing automatically if scene is unloaded
        /// </summary>
        public void PlaySFX(AudioClip clip, float volumeScale = 1.0f, float pitch = 1.0f)
        {
            var source = _sfxSourcePool.Get();
            source.pitch = pitch;
            source.clip = clip;
            Observable.Timer(System.TimeSpan.FromSeconds(clip.length))
                .TakeUntil(_ => source != null)
                .Take(1)
                .Subscribe(x => _sfxSourcePool.Release(source));
            
            source.Play();
        }
    }
}
