using System;
using UnityEngine;

namespace LostKaiju.Boilerplates.FSM
{
    public class FiniteStateMachine : BaseFiniteStateMachine
    {
        public override void ChangeState(Type stateType)
        {
            if (_currentStateType == stateType)
                return;

            if (_states.TryGetValue(stateType, out var newState))
            {
                CurrentState?.Exit();
                CurrentState = newState;
                _currentStateType = stateType;
                CurrentState.Enter();
                Debug.Log($"FSM: {_currentStateType.Name} --> {stateType.Name}");
            }
            else
            {
                Debug.LogWarning($"FSM warnig: Can`t find '{stateType.Name}' state for transition");
            }
        }
    }
}
