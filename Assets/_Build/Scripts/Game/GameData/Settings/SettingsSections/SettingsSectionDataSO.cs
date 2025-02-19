using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "SettingsSectionDataSO", menuName = "Scriptable Objects/Settings/SettingsSectionDataSO")]
    public class SettingsSectionDataSO : ScriptableObject, ISettingsSectionData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Label { get; private set; }
        public ISettingBarData[] SettingBarsData => _settingBarsDataSO;

        [SerializeField] private SettingBarDataSO[] _settingBarsDataSO;
    }
}