using System;
using UnityEngine;
using R3;

using LostKaiju.Game.Providers.InputState;
using LostKaiju.Game.World.Missions.Triggers;
using LostKaiju.Infrastructure.SceneBootstrap.Context;

namespace LostKaiju.Infrastructure.Managers
{
    public class SceneTransitionManager : IDisposable
    {
        public Observable<Unit> ExitSignal => _exitSignal;

        private readonly PlayerManager _playerManager;
        private readonly SubSceneTrigger[] _subSceneTriggers;
        private readonly PlayerHeroTrigger _missionExitAreaTrigger;

        private GameplayEnterContext _gameplayEnterContext;
        private MissionEnterContext _missionEnterContext; // send from bootstrap
        private MissionExitContext _missionExitContext; // exit from the current mission to subscene
        private readonly Subject<Unit> _exitSignal = new();
        private readonly CompositeDisposable _disposables = new();

        public SceneTransitionManager(
            PlayerManager playerManager,
            SubSceneTrigger[] subSceneTriggers,
            PlayerHeroTrigger missionExitAreaTrigger)
        {
            _playerManager = playerManager;
            _subSceneTriggers = subSceneTriggers;
            _missionExitAreaTrigger = missionExitAreaTrigger;
        }

        public void Initialize(MissionEnterContext missionEnterContext, MissionExitContext missionExitContext)
        {
            _missionEnterContext = missionEnterContext;
            _gameplayEnterContext = missionEnterContext.GameplayEnterContext;
            _missionExitContext = missionExitContext;

            if (_subSceneTriggers != null)
            {
                SetupSubSceneTriggers();
            }

            if (_missionExitAreaTrigger != null)
            {
                SetupMissionExitTrigger(missionExitContext.MissionEnterContext);
            }
        }

        private void SetupSubSceneTriggers()
        {
            var shouldSkip = !String.IsNullOrEmpty(_missionEnterContext.FromTriggerId); // TODO: change to confirm action

            foreach (var subTrigger in _subSceneTriggers)
            {
                if (subTrigger == null) continue;

                var onTriggerEnter = shouldSkip && subTrigger.ToSceneName == _missionEnterContext.FromTriggerId
                    ? subTrigger.OnEnter.Skip(1)
                    : subTrigger.OnEnter;

                onTriggerEnter.Take(1)
                    .Subscribe(_ => HandleSubSceneTransition(subTrigger))
                    .AddTo(_disposables);
            }
        }

        private void SetupMissionExitTrigger(MissionEnterContext toMissionEnterContext)
        {
            if (!String.IsNullOrEmpty(_missionEnterContext.FromMissionSceneName)) // is returning from subscene
            {
                _missionExitAreaTrigger.OnEnter.Take(1)
                    .Subscribe(_ => HandleReturnToMainScene(toMissionEnterContext))
                    .AddTo(_disposables);
            }
            else
            {
                _missionExitAreaTrigger.OnEnter.Take(1)
                    .Subscribe(_ => HandleMissionComplete())
                    .AddTo(_disposables);
            }
        }

        private void HandleSubSceneTransition(SubSceneTrigger trigger)
        {
            PrepareTransition();
            var toMissionEnterContext = _missionExitContext.MissionEnterContext;
            toMissionEnterContext.FromMissionSceneName = _gameplayEnterContext.LevelSceneName;
            toMissionEnterContext.PlayerPosition = _playerManager.PlayerTransform.position;
            toMissionEnterContext.FromTriggerId = trigger.ToSceneName;
            _missionExitContext.ToMissionSceneName = trigger.ToSceneName;
            _exitSignal.OnNext(Unit.Default);
        }

        private void HandleReturnToMainScene(MissionEnterContext toMissionEnterContext)
        {
            PrepareTransition();
            toMissionEnterContext.PlayerPosition = _missionEnterContext.PlayerPosition;
            toMissionEnterContext.FromTriggerId = _missionEnterContext.FromTriggerId;
            _missionExitContext.ToMissionSceneName = _missionEnterContext.FromMissionSceneName;
            _exitSignal.OnNext(Unit.Default);
        }

        private void HandleMissionComplete()
        {
            PrepareTransition();
            Debug.Log("Mission completed signal");
            _gameplayEnterContext.MissionCompletionSignal.OnNext(Unit.Default);   
            // _exitGameplaySignal.OnNext(Unit.Default);  
        }

        private void PrepareTransition()
        {
            // _playerManager.DisposePlayer();
            _playerManager.SetInvincible(true);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}