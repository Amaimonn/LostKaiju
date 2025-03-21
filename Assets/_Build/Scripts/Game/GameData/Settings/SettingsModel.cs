using R3;

namespace LostKaiju.Game.GameData.Settings
{
    public class SettingsModel : Model<SettingsState>
    {
#region Sound settings
        public readonly ReactiveProperty<int> MusicVolume;
        public readonly ReactiveProperty<bool> IsMusicEnabled;
        public readonly ReactiveProperty<int> SfxVolume;
        public readonly ReactiveProperty<bool> IsSfxEnabled;
#endregion

#region Video settings
        public readonly ReactiveProperty<int> Brightness;
        public readonly ReactiveProperty<bool> IsPostProcessingEnabled;
        public readonly ReactiveProperty<bool> IsBloomEnabled;
        public readonly ReactiveProperty<bool> IsAntiAliasingEnabled;
#endregion

        public SettingsModel(SettingsState state) : base(state)
        {
            MusicVolume = new ReactiveProperty<int>(state.MusicVolume);
            MusicVolume.Skip(1).Subscribe(x => state.MusicVolume = x);

            IsMusicEnabled = new ReactiveProperty<bool>(state.IsMusicEnabled);
            IsMusicEnabled.Skip(1).Subscribe(x => state.IsMusicEnabled = x);

            SfxVolume = new ReactiveProperty<int>(state.SfxVolume);
            SfxVolume.Skip(1).Subscribe(x => state.SfxVolume = x);

            IsSfxEnabled = new ReactiveProperty<bool>(state.IsSfxEnabled);
            IsSfxEnabled.Skip(1).Subscribe(x => state.IsSfxEnabled = x);

            Brightness = new ReactiveProperty<int>(state.Brightness);
            Brightness.Skip(1).Subscribe(x => state.Brightness = x);

            IsPostProcessingEnabled = new ReactiveProperty<bool>(state.IsPostProcessingEnabled);
            IsPostProcessingEnabled.Skip(1).Subscribe(x => state.IsPostProcessingEnabled = x);

            IsBloomEnabled = new ReactiveProperty<bool>(state.IsBloomEnabled);
            IsBloomEnabled.Skip(1).Subscribe(x => state.IsBloomEnabled = x);

            IsAntiAliasingEnabled = new ReactiveProperty<bool>(state.IsAntiAliasingEnabled);
            IsAntiAliasingEnabled.Skip(1).Subscribe(x => state.IsAntiAliasingEnabled = x);
        }
    }
}
