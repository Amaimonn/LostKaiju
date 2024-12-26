using UnityEngine;

using LostKaiju.Gameplay.Player.Data;
using LostKaiju.Gameplay.Player.Behaviour;
using LostKaiju.Gameplay.Creatures.Presenters;

namespace LostKaiju.Gameplay.Configs 
{
    [CreateAssetMenu(fileName = "PlayerPresenterSO", menuName = "Scriptable Objects/Player Presenter SO")]
    public class PlayerPresenterSO : CreaturePresenterSO
    {
        public override CreaturePresenter GetPresenter() => new PlayerPresenter(_BaseCharacterInputData);

        [SerializeField] private PlayerControlsData _BaseCharacterInputData;
    }
}