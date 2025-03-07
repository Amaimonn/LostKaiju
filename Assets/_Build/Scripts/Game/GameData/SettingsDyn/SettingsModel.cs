using System;
using System.Collections.Generic;
using R3;

namespace LostKaiju.Game.GameData.SettingsDyn
{
    public class SettingsModel : Model<SettingsState>
    {
        private readonly Dictionary<string, ReactiveProperty<float>> _floatSettings = new();
        private readonly Dictionary<string, ReactiveProperty<bool>> _boolSettings = new();

        public SettingsModel(SettingsState state) : base(state)
        {
            var floatSettings = state.FloatSettings;
            foreach (var floatSetting in floatSettings)
            {
                var settingKey = floatSetting.Key;
                AddFloatSetting(settingKey, floatSetting.Value, x => floatSettings[settingKey] = x);
            }

            var boolSettings = state.BoolSettings;
            foreach (var boolSetting in boolSettings)
            {
                var settingKey = boolSetting.Key;
                AddBoolSetting(settingKey, boolSetting.Value, x => boolSettings[settingKey] = x);
            }
        }

        private void AddFloatSetting(string key, float initialValue, Action<float> updateAction)
        {
            var property = new ReactiveProperty<float>(initialValue);
            property.Skip(1).Subscribe(updateAction);
            _floatSettings[key] = property;
        }

        private void AddBoolSetting(string key, bool initialValue, Action<bool> updateAction)
        {
            var property = new ReactiveProperty<bool>(initialValue);
            property.Skip(1).Subscribe(updateAction);
            _boolSettings[key] = property;
        }

        public ReactiveProperty<float> GetFloatSetting(string key) => _floatSettings[key];
        public ReactiveProperty<bool> GetBoolSetting(string key) => _boolSettings[key];
    }
}