using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "SliderSettingDataSO", menuName = "Scriptable Objects/Settings/SliderSettingDataSO")]
    public class SliderSettingDataSO : SettingBarDataSO, ISliderSettingData
    {
        [field: SerializeField] public float MinValue { get; private set; }
        [field: SerializeField] public float MaxValue { get; private set; }
    }
}