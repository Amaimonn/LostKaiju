using UnityEngine;

using LostKaiju.Gameplay.Player.Data;
using LostKaiju.Gameplay.Creatures.Presenters;
using LostKaiju.Gameplay.Player;

namespace LostKaiju.Gameplay.Configs 
{
    [CreateAssetMenu(fileName = "PlayerPresenterSO", menuName = "Scriptable Objects/Player Presenter SO")]
    public class PlayerPresenterSO : CreaturePresenterSO
    {
        public override CreaturePresenter GetPresenter() => new PlayerRootPresenter(_BaseCharacterInputData);

        [SerializeField] private PlayerControlsData _BaseCharacterInputData;
    }
}