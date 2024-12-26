using System;
using UnityEngine;

namespace LostKaiju.Models.FSM
{
    public class BaseFiniteStateMachine : FiniteStateMachine
    {
        public BaseFiniteStateMachine(Type startStateType) : base(startStateType)
        {
        }

        public override void ChangeState(Type stateType)
        {
            if (_currentStateType == stateType)
                return;

            if (_states.TryGetValue(stateType, out var newState))
            {
                Debug.Log($"{_currentStateType.Name} --> {stateType.Name}");
                CurrentState?.Exit();
                CurrentState = newState;
                _currentStateType = stateType;
                CurrentState.Enter();
            }
            else
            {
                Debug.Log($"There is no {stateType.Name} in FSM");
            }
        }
    }
}
