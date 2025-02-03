using R3;

namespace LostKaiju.Game.GameData.Settings
{
    public class SettingsModel : Model<SettingsState>
    {
        public ReactiveProperty<float> SoundVolume;
        public ReactiveProperty<float> SFXVolume;

        public SettingsModel(SettingsState state) : base(state)
        {
            SoundVolume = new ReactiveProperty<float>(state.SoundVolume);
            SoundVolume.Skip(1).Subscribe(x => state.SoundVolume = x);

            SFXVolume = new ReactiveProperty<float>(state.SFXVolume);
            SFXVolume.Skip(1).Subscribe(x => state.SFXVolume = x);
        }
    }
}
