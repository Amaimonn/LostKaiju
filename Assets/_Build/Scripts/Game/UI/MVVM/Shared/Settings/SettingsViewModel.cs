using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared
{
    public class SettingsViewModel : IViewModel
    {
        private readonly SettingsModel _model;

        public SettingsViewModel(SettingsModel model)
        {
            _model = model;
        }
    }
}