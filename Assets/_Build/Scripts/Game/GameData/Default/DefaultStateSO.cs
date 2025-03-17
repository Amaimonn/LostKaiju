using UnityEngine;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.GameData.Heroes;

namespace LostKaiju.Game.GameData.Default
{
    [CreateAssetMenu(fileName = "DefaultStateSO", menuName = "Scriptable Objects/DefaultStateSO")]
    public class DefaultStateSO : ScriptableObject, IDefaultState
    {    
        [field: SerializeField] public SettingsState Settings { get; private set; }
        [field: SerializeField] public CampaignState Campaign { get; private set; }
        [field: SerializeField] public HeroesState Heroes { get; private set; }
    }
}