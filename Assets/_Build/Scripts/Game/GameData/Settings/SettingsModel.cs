using R3;

namespace LostKaiju.Game.GameData.Settings
{
    public class SettingsModel : Model<SettingsState>
    {
        public readonly ReactiveProperty<float> SoundVolume;
        public readonly ReactiveProperty<bool> IsSoundEnabled;
        public readonly ReactiveProperty<float> SfxVolume;
        public readonly ReactiveProperty<bool> IsSfxEnabled;
        public readonly ReactiveProperty<float> Brightness;

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
        }
    }
}
