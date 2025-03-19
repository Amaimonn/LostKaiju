using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "SliderSettingDataSO", menuName = "Scriptable Objects/Settings/SliderSettingDataSO")]
    public class SliderSettingDataSO : SettingBarDataSO, ISliderSettingData
    {
        [field: SerializeField] public int MinValue { get; private set; }
        [field: SerializeField] public int MaxValue { get; private set; }
    }
}