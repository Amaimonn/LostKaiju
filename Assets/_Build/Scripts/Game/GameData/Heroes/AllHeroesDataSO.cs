using UnityEngine;

namespace LostKaiju.Game.GameData.Heroes
{
    [CreateAssetMenu(fileName = "AllHeroesDataSO", menuName = "Scriptable Objects/Heroes/AllHeroesDataSO")]
    public class AllHeroesDataSO : ScriptableObject, IAllHeroesData
    {
        public IHeroData[] AllData => _heroesData;
        [field: SerializeField] private HeroDataSO[] _heroesData;
    }
}
