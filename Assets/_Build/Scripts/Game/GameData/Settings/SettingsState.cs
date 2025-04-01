using System;
using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [Serializable]
    public class SettingsState : IVersioned, ICopyable<SettingsState>
    {
        public int Version { get => _version; set => _version = value; }
        [SerializeField] private int _version = 1;
        [Range(0, 10)] public int MusicVolume;
        public bool IsMusicEnabled;
        [Range(0, 10)] public int SfxVolume;
        public bool IsSfxEnabled;

        [Range(-5, 5)] public int Brightness;
        public bool IsPostProcessingEnabled;
        public bool IsBloomEnabled;
        public bool IsFilmGrainEnabled;
        public bool IsAntiAliasingEnabled;

        public int LanguageIndex;

        public SettingsState Copy()
        {
            var copy = (SettingsState)MemberwiseClone();
            return copy;
        }
    }
}
