using UnityEngine;

using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.GameData.Default
{
    [CreateAssetMenu(fileName = "DefaultStateSO", menuName = "Scriptable Objects/DefaultStateSO")]
    public class DefaultStateSO : ScriptableObject, IDefaultState
    {    
        [field: SerializeField] public SettingsState SettingsState { get; private set; }
        [field: SerializeField] public CampaignState CampaignState { get; private set; }
    }
}