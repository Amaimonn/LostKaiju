using UnityEngine;

using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.GameData.SettingsDyn
{
    [CreateAssetMenu(fileName = "SettingsSectionDataSO", menuName = "Scriptable Objects/SettingsDyn/SettingsSectionDataSO")]
    public class SettingsSectionDataSO : ScriptableObject, ISettingsSectionData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Label { get; private set; }
        public ISettingBarData[] SettingBarsData => _settingBarsDataSO;

        [SerializeField] private SettingBarDataSO[] _settingBarsDataSO;
    }
}