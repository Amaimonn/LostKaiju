using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Services.Saves;

namespace LostKaiju.Game.Providers.GameState
{
    // public partial class SavesYG
    // {
    //     public SettingsState Settings;
    //     public CampaignState Campaign;
    // }
    
    // public class GameStateProviderYG : IGameStateProvider
    // {
    //     public SettingsState Settings {get;}
    //     public CampaignState Campaign {get;}

    //     public Task SaveCampaignAsync()
    //     {
    //         // YG2.saves.Campaign = Campaign;
    //         // YG2.SaveProgress();
    //         return Task.CompletedTask;
    //     }

    //     public Task SaveSettingsAsync()
    //     {
    //         // YG2.saves.Settings = Settings;
    //         // YG2.SaveProgress()
    //         return Task.CompletedTask;
    //     }
    // }
}