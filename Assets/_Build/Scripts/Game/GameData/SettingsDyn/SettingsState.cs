using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostKaiju.Game.GameData.SettingsDyn
{
    [Serializable]
    public class SettingsState : IVersioned, ISerializationCallbackReceiver
    {
        public int Version => _version;
        [SerializeField] private int _version = 1;
        [NonSerialized] public Dictionary<string, float> FloatSettings = new();
        [NonSerialized] public Dictionary<string, bool> BoolSettings = new();

        public List<FloatPair> _floatSettingsList;
        public List<BoolPair> _boolSettingsList;

        public void OnBeforeSerialize()
        {
            _floatSettingsList = new();
            foreach (var kvp in FloatSettings)
                _floatSettingsList.Add(new FloatPair(kvp.Key, kvp.Value));

            _boolSettingsList = new();
            foreach (var kvp in BoolSettings)
                _boolSettingsList.Add(new BoolPair(kvp.Key, kvp.Value));
        }

        public void OnAfterDeserialize()
        {
            if (_floatSettingsList != null)
            {
                foreach(var pair in _floatSettingsList)
                    FloatSettings.Add(pair.Key, pair.Value);
                _floatSettingsList.Clear();
            }
            
            if (_boolSettingsList != null)
            {
                foreach (var pair in _boolSettingsList)
                    BoolSettings.Add(pair.Key, pair.Value);
                _boolSettingsList.Clear();
            }
        }

        [Serializable]
        public struct FloatPair
        {
            public string Key;
            public float Value;

            public FloatPair(string key, float value)
            {
                Key = key;
                Value = value;
            }
        }

        [Serializable]
        public struct BoolPair
        {
            public string Key;
            public bool Value;

            public BoolPair(string key, bool value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}