using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "ArrowsSettingDataSO", menuName = "Scriptable Objects/Settings/ArrowsSettingDataSO")]
    public class ArrowsSettingDataSO : SettingBarDataSO, IArrowsSettingData
    {
        [field: SerializeField] public string[] Options { get; private set; }
    }
}