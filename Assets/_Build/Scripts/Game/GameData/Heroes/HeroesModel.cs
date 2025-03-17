using R3;

namespace LostKaiju.Game.GameData.Heroes
{
    public class HeroesModel : Model<HeroesState>
    {
        public IAllHeroesData AllHeroesData { get; }
        public readonly ReactiveProperty<string> SelectedHeroId;

        public HeroesModel(HeroesState state, IAllHeroesData allHeroesData) : base(state)
        {
            AllHeroesData = allHeroesData;

            SelectedHeroId = new ReactiveProperty<string>(state.SelectedHeroId);
            SelectedHeroId.Skip(1).Subscribe(x => state.SelectedHeroId = x);
        }
    }
}