using System.Collections.Generic;
using UnityEngine;
using R3;

using LostKaiju.Boilerplates.Locator;
using LostKaiju.Utils;
using LostKaiju.Services.Inputs;
using LostKaiju.Boilerplates.FSM;
using LostKaiju.Boilerplates.FSM.FiniteTransitions;
using LostKaiju.Game.Player.Behaviour.PlayerControllerStates;
using LostKaiju.Game.Creatures.Features;
using LostKaiju.Game.Player.Data.StateParameters;

namespace LostKaiju.Game.Player.Behaviour.StateBinders
{
    [CreateAssetMenu(fileName = "DashStateBinderSO", menuName = "Scriptable Objects/DashStateBinderSO")]
    public class DashStateBinderSO : FiniteStateBinderSO
    {
        [SerializeField] private DashParameters _parameters;

        public (FiniteState, IEnumerable<IFiniteTransition>) Bind(Holder<ICreatureFeature> features, Rigidbody2D rigidbody)
        {
            var inputProvider = ServiceLocator.Instance.Get<IInputProvider>();

            var flipper = features.Resolve<Flipper>();

            var dashState = new DashState();
            dashState.Init(_parameters, rigidbody, Observable.EveryValueChanged(flipper, x => x.IsLookingToTheRight));

            float _waitToDash = 0;
            dashState.OnEnter.Subscribe(_ => {
                _waitToDash = _parameters.Cooldown;
                Observable.EveryUpdate().TakeWhile(_ => _waitToDash > 0).Subscribe(_ => _waitToDash -= Time.deltaTime);
            });

            var transitions = new IFiniteTransition[]
            {
                new FiniteTransition<WalkState, DashState>(() => inputProvider.GetShift && _waitToDash <= 0),
                new FiniteTransition<DashState, IdleState>(() => dashState.IsCompleted.CurrentValue),
                new FiniteTransition<IdleState, DashState>(() => inputProvider.GetShift && _waitToDash <= 0),
                new FiniteTransition<JumpState, DashState>(() => inputProvider.GetShift && _waitToDash <= 0),
            };
            return (dashState, transitions);
        }
    }
}
