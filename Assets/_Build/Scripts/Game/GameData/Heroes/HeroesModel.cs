using System.Collections.Generic;
using System.Linq;
using R3;

namespace LostKaiju.Game.GameData.Heroes
{
    public class HeroesModel : Model<HeroesState>
    {
        public IAllHeroesData AllHeroesData { get; }
        public Dictionary<string, IHeroData> HeroDataMap { get; }
        public readonly ReactiveProperty<IHeroData> SelectedHeroData;

        public HeroesModel(HeroesState state, IAllHeroesData allHeroesData) : base(state)
        {
            AllHeroesData = allHeroesData;
            HeroDataMap = allHeroesData.AllData.ToDictionary(x => x.Id);

            var selectedHeroData = AllHeroesData.AllData.FirstOrDefault(x => x.Id == state.SelectedHeroId);
            SelectedHeroData = new ReactiveProperty<IHeroData>(selectedHeroData);
            SelectedHeroData.Skip(1).Subscribe(x => state.SelectedHeroId = x.Id);
        }
    }
}