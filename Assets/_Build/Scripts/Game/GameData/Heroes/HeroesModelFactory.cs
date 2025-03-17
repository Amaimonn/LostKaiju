using System.Threading.Tasks;
using UnityEngine;
using R3;

using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.GameData.Heroes
{
    public class HeroesModelFactory : IAsyncModelFactory<HeroesModel>
    {
        public Observable<HeroesModel> OnProduced => _onProduced;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly Subject<HeroesModel> _onProduced = new();

        public HeroesModelFactory(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public async Task<HeroesModel> GetModelAsync()
        {
            if (_gameStateProvider.Heroes == null)
            {
                await _gameStateProvider.LoadHeroesAsync();
            }
            var heroesDataSORequest = Resources.LoadAsync<AllHeroesDataSO>(Paths.ALL_HEROES_DATA);
            await heroesDataSORequest;

            var heroesDataSO = heroesDataSORequest.asset as AllHeroesDataSO;
            var heroesState = _gameStateProvider.Heroes;
            var heroesModel = new HeroesModel(heroesState, heroesDataSO);

            _onProduced.OnNext(heroesModel);

            return heroesModel;
        }
    }
}