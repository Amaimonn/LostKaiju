using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "SettingsDataSO", menuName = "Scriptable Objects/Settings/SettingsDataSO")]
    public class SettingsDataSO : ScriptableObject, ISettingsData
    {
        [field: SerializeField] public string Label { get; private set; }
        public ISettingsSectionData[] SectionsData => _sectionsDataSO;

        [SerializeField] private SettingsSectionDataSO[] _sectionsDataSO;
    }
}