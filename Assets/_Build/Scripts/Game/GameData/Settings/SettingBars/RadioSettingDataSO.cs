using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "RadioSettingDataSO", menuName = "Scriptable Objects/Settings/RadioSettingDataSO")]
    public class RadioSettingDataSO : SettingBarDataSO, IRadioSettingData
    {
        [field: SerializeField] public string[] Options { get; private set; }
    }
}