using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    public abstract class SettingBarDataSO : ScriptableObject, ISettingBarData
    {
        [field: SerializeField] public string Label { get; private set; }
    }
}