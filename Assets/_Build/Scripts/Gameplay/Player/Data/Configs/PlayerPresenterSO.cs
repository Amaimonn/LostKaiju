using UnityEngine;

using LostKaiju.Gameplay.Creatures.Presenters;
using LostKaiju.Gameplay.Configs;

namespace LostKaiju.Gameplay.Player.Data.Configs
{
    [CreateAssetMenu(fileName = "PlayerPresenterSO", menuName = "Scriptable Objects/Player Presenter SO")]
    public class PlayerPresenterSO : CreaturePresenterSO
    {
        public override CreaturePresenter GetPresenter() => new PlayerRootPresenter(_playerControlsData, _playerDefenceData);

        [SerializeField] private PlayerControlsData _playerControlsData;
        [SerializeField] private PlayerDefenceData _playerDefenceData;
    }
}