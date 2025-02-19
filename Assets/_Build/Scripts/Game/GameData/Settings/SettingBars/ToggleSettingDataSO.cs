using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "ToggleSettingDataSO", menuName = "Scriptable Objects/Settings/ToggleSettingDataSO")]
    public class ToggleSettingDataSO : SettingBarDataSO, IToggleSettingData
    {
    }
}