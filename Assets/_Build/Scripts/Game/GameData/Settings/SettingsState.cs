using System;
using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [Serializable]
    public class SettingsState : IVersioned
    {
        public int Version => _version;
        [SerializeField] private int _version = 1;

        public float SoundVolume;
        public bool IsSoundEnabled;
        public float SfxVolume;
        public bool IsSfxEnabled;
        public float Brightness;
        public bool IsHighBloomQuality;
        public bool IsAntiAliasingEnabled;
    }
}
