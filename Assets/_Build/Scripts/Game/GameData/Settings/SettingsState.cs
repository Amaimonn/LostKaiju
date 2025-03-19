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
        public int Brightness;
        public bool IsPostProcessingEnabled;
        public bool IsHighBloomQuality;
        public bool IsAntiAliasingEnabled;

        public SettingsState Copy()
        {
            var copy = (SettingsState)MemberwiseClone();
            return copy;
        }
    }
}
