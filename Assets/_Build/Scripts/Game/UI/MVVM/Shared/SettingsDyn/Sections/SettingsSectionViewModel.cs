using System;
using System.Collections.Generic;
using System.Linq;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.SettingsDyn;
using System.Diagnostics;

namespace LostKaiju.Game.UI.MVVM.Shared.SettingsDyn
{
    public abstract class SettingsSectionViewModel : IViewModel, IDisposable
    {
        public readonly ISettingsSectionData Data;
        public ReadOnlyReactiveProperty<bool> IsAnyChanges = new ReactiveProperty<bool>(false);
        public Dictionary<string, ReadOnlyReactiveProperty<float>> FloatSettings { get; set; } = new();
        public Dictionary<string, ReadOnlyReactiveProperty<bool>> BoolSettings { get; set; } = new();
        public Dictionary<string, Action<float>> FloatMethods { get; set; } = new();
        public Dictionary<string, Action<bool>> BoolMethods { get; set; } = new();

        protected readonly SettingsModel _model;
        protected readonly List<Action> _resetActionsList = new();
        protected readonly List<Action> _applyActionsList = new();
        protected readonly Dictionary<string, float> _cachedFloatValues = new();
        protected readonly Dictionary<string, bool> _cachedBoolValues = new();
        protected readonly List<Action> _cacheActionsList = new();
        protected CompositeDisposable _disposables;

        public SettingsSectionViewModel(SettingsModel model, ISettingsSectionData sectionData)
        {
            _model = model;
            Data = sectionData;
            _disposables = new();
        }

        public virtual void ApplyChanges()
        {
            foreach (var applyAction in _applyActionsList)
                applyAction();
            CacheSettings();
        }

        public virtual void ResetSettings()
        {
            foreach (var resetAction in _resetActionsList)
                resetAction();
        }

        protected void InitSettings(IEnumerable<string> floatNames, IEnumerable<string> boolNames)
        {
            var setAfterApplyMap = new Dictionary<string, bool>();

            foreach (var data in Data.SettingBarsData)
                setAfterApplyMap.Add(data.NameId, data.SetAfterApply);
            
            foreach (var floatSettingName in floatNames)
                if (setAfterApplyMap.ContainsKey(floatSettingName))
                    InitFloatSetting(floatSettingName, setAfterApplyMap[floatSettingName]);

            foreach (var boolSettingName in boolNames)
                if (setAfterApplyMap.ContainsKey(boolSettingName))
                    InitBoolSetting(boolSettingName, setAfterApplyMap[boolSettingName]);

            var changesReactive = new List<Observable<bool>>();

            foreach (var setting in FloatSettings)
                changesReactive.Add(setting.Value.Select(x => x != _cachedFloatValues[setting.Key]));
            
            foreach (var setting in BoolSettings)
                changesReactive.Add(setting.Value.Select(x => x != _cachedBoolValues[setting.Key]));

            IsAnyChanges = Observable.CombineLatest(changesReactive)
                .Select(x => x.Any(t => t == true))
                .ToReadOnlyReactiveProperty();
        }

        protected ReactiveProperty<float> CreateReactiveFloatSetting(ReactiveProperty<float> modelFloatSetting)
        {
            var setting = new ReactiveProperty<float>(modelFloatSetting.Value);
            modelFloatSetting.Skip(1).Subscribe(x => setting.Value = x).AddTo(_disposables);

            return setting;
        }

        protected ReactiveProperty<bool> CreateReactiveBoolSetting(ReactiveProperty<bool> modelBoolSetting)
        {
            var setting = new ReactiveProperty<bool>(modelBoolSetting.Value);
            modelBoolSetting.Skip(1).Subscribe(x => setting.Value = x).AddTo(_disposables);

            return setting;
        }

        protected void InitFloatSetting(string settingKey, bool setAfterApply = false)
        {
            var modelFloatSetting = _model.GetFloatSetting(settingKey);
            var setting = CreateReactiveFloatSetting(modelFloatSetting);
            FloatSettings.Add(settingKey, setting);

            AddCacheFloatAction(settingKey, modelFloatSetting);
            if (setAfterApply)
            {
                _applyActionsList.Add(() => modelFloatSetting.Value = setting.Value);
                _resetActionsList.Add(() => modelFloatSetting.OnNext(_cachedFloatValues[settingKey]));
                FloatMethods.Add(settingKey, x => setting.Value = x);
            }
            else
            {
                _resetActionsList.Add(() => modelFloatSetting.Value = _cachedFloatValues[settingKey]);
                FloatMethods.Add(settingKey, x => modelFloatSetting.Value = x);
            }
        }

        protected void InitBoolSetting(string settingKey, bool setAfterApply = false)
        {
            var modelBoolSetting = _model.GetBoolSetting(settingKey);
            var setting = CreateReactiveBoolSetting(modelBoolSetting);
            BoolSettings.Add(settingKey, setting);

            AddCacheBoolAction(settingKey, modelBoolSetting);
            if (setAfterApply)
            {
                _applyActionsList.Add(() => modelBoolSetting.Value = setting.Value);
                _resetActionsList.Add(() => modelBoolSetting.OnNext(_cachedBoolValues[settingKey]));
                BoolMethods.Add(settingKey, x => setting.Value = x);
            }
            else
            {
                _resetActionsList.Add(() => modelBoolSetting.Value = _cachedBoolValues[settingKey]);
                BoolMethods.Add(settingKey, x => modelBoolSetting.Value = x);
            }
        }

        protected void AddCacheFloatAction(string settingKey, ReactiveProperty<float> modelSetting)
        {
            _cachedFloatValues.Add(settingKey, modelSetting.Value);
            _cacheActionsList.Add(() =>_cachedFloatValues[settingKey] = modelSetting.Value);
        }

        protected void AddCacheBoolAction(string settingKey, ReactiveProperty<bool> modelSetting)
        {
            _cachedBoolValues.Add(settingKey, modelSetting.Value);
            _cacheActionsList.Add(() =>_cachedBoolValues[settingKey] = modelSetting.Value);
        }

        protected virtual void CacheSettings()
        {
            foreach (var cacheAction in _cacheActionsList)
                cacheAction();
        }

        public virtual void Dispose()
        {
            if (_disposables != null)
            {
                _disposables.Dispose();
                _disposables = null;
            }
        }
    }
}