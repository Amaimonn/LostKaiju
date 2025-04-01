using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Shared.Settings;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        private readonly CampaignNavigationBinder _campaignNavigationBinder;
        private readonly SettingsBinder _settingsBinder;
        private readonly HeroSelectionBinder _heroSelectionBinder;
        
        public HubViewModel(CampaignNavigationBinder campaignNavigationBinder, SettingsBinder settingsBinder, 
            HeroSelectionBinder heroSelectionBinder)
        {
            _campaignNavigationBinder = campaignNavigationBinder;
            _settingsBinder = settingsBinder;
            _heroSelectionBinder = heroSelectionBinder;
        }

        public void OpenCampaign()
        {
            _campaignNavigationBinder.TryBindAndOpen(out _);
        }

        public void OpenHeroSelection()
        {
            _heroSelectionBinder.TryBindAndOpen(out _);
        }

        public void OpenSettings()
        {
            _settingsBinder.TryBindAndOpen(out _);
        }
    }
}