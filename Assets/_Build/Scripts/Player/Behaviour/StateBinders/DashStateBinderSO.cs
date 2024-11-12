using System;
using System.Collections.Generic;
using UnityEngine;
using R3;

using LostKaiju.Architecture.Services;
using LostKaiju.Architecture.Utils;
using LostKaiju.Architecture.Providers;
using LostKaiju.Architecture.FSM;
using LostKaiju.Architecture.FSM.FiniteTransitions;
using LostKaiju.Player.Behaviour.PlayerControllerStates;
using LostKaiju.Player.Data.StateParameters;
using LostKaiju.Entities.EntityFeatures;

namespace LostKaiju.Player.Behaviour.StateBinders
{
    [CreateAssetMenu(fileName = " DashStateBinderSO", menuName = "Scriptable Objects/ DashStateBinderSO")]
    [Serializable]
    public class DashStateBinderSO : FiniteStateBinderSO
    {
        [SerializeField] private DashParameters _parameters;

        public (FiniteState, IEnumerable<IFiniteTransition>) Bind(Holder<IEntityFeature> features)
        {
            var inputProvider = ServiceLocator.Current.Get<IInputProvider>();

            var flipper = features.Resolve<Flipper>();

            var dashState = new DashState();
            dashState.Init(_parameters, Observable.EveryValueChanged(flipper, x => x.IsLooksToTheRight));

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
