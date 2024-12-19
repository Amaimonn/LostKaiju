using UnityEngine;

using LostKaiju.Player.Data;
using LostKaiju.Player.Behaviour;
using LostKaiju.Creatures.Presenters;

namespace LostKaiju.Configs 
{
    [CreateAssetMenu(fileName = "PlayerPresenterSO", menuName = "Scriptable Objects/Player Presenter SO")]
    public class PlayerPresenterSO : CreaturePresenterSO
    {
        public override CreaturePresenter GetPresenter() => new PlayerPresenter(_BaseCharacterInputData);

        [SerializeField] private PlayerControlsData _BaseCharacterInputData;
    }
}