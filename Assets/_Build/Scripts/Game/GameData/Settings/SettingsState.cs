using System;
using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [Serializable]
    public class SettingsState : IVersioned, ICopyable<SettingsState>
    {
        public int Version { get => _version; set => _version = value; }
        [SerializeField] private int _version = 1;

        public float SoundVolume;
        public bool IsSoundEnabled;
        public float SfxVolume;
        public bool IsSfxEnabled;
        public float Brightness;
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
