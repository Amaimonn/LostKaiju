using R3;

namespace LostKaiju.Game.GameData.Settings
{
    public class SettingsModel : Model<SettingsState>
    {
#region Sound settings
        public readonly ReactiveProperty<float> SoundVolume;
        public readonly ReactiveProperty<bool> IsSoundEnabled;
        public readonly ReactiveProperty<float> SfxVolume;
        public readonly ReactiveProperty<bool> IsSfxEnabled;
#endregion

#region Video settings
        public readonly ReactiveProperty<float> Brightness;
        public readonly ReactiveProperty<bool> IsPostProcessingEnabled;
        public readonly ReactiveProperty<bool> IsHighBloomQuality;
        public readonly ReactiveProperty<bool> IsAntiAliasingEnabled;
#endregion

        public SettingsModel(SettingsState state) : base(state)
        {
            SoundVolume = new ReactiveProperty<float>(state.SoundVolume);
            SoundVolume.Skip(1).Subscribe(x => state.SoundVolume = x);

            IsSoundEnabled = new ReactiveProperty<bool>(state.IsSoundEnabled);
            IsSoundEnabled.Skip(1).Subscribe(x => state.IsSoundEnabled = x);

            SfxVolume = new ReactiveProperty<float>(state.SfxVolume);
            SfxVolume.Skip(1).Subscribe(x => state.SfxVolume = x);

            IsSfxEnabled = new ReactiveProperty<bool>(state.IsSfxEnabled);
            IsSfxEnabled.Skip(1).Subscribe(x => state.IsSfxEnabled = x);

            Brightness = new ReactiveProperty<float>(state.Brightness);
            Brightness.Skip(1).Subscribe(x => state.Brightness = x);

            IsPostProcessingEnabled = new ReactiveProperty<bool>(state.IsPostProcessingEnabled);
            IsPostProcessingEnabled.Skip(1).Subscribe(x => state.IsPostProcessingEnabled = x);

            IsHighBloomQuality = new ReactiveProperty<bool>(state.IsHighBloomQuality);
            IsHighBloomQuality.Skip(1).Subscribe(x => state.IsHighBloomQuality = x);

            IsAntiAliasingEnabled = new ReactiveProperty<bool>(state.IsAntiAliasingEnabled);
            IsAntiAliasingEnabled.Skip(1).Subscribe(x => state.IsAntiAliasingEnabled = x);
        }
    }
}
