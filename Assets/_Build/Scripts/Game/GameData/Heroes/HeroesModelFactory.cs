using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using R3;

using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.GameData.Heroes
{
    public class HeroesModelFactory : ILoadableModelFactory<HeroesModel>
    {
        public Observable<HeroesModel> OnProduced => _onProduced;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly Subject<HeroesModel> _onProduced = new();
        private AsyncOperationHandle<AllHeroesDataSO> _handle;

        public HeroesModelFactory(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public void GetModelOnLoaded(Action<HeroesModel> onLoaded)
        {
            _handle = Addressables.LoadAssetAsync<AllHeroesDataSO>(Paths.ALL_HEROES_DATA);
            _handle.Completed += (handler) =>
            {
                var heroesDataSO = handler.Result;
                var heroesState = _gameStateProvider.Heroes;
                var heroesModel = new HeroesModel(heroesState, heroesDataSO);
                onLoaded(heroesModel);
                _onProduced.OnNext(heroesModel);
            };
        }

        public void Release()
        {
            Addressables.Release(_handle);
        }
    }
}