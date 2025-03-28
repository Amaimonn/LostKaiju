using System;
using UnityEngine;
using VContainer;

namespace LostKaiju.Game.World.Enemy
{
    [Serializable]
    public class EnemyInjector
    {
        [SerializeField] private EnemyRootPresenter[] _enemies = new EnemyRootPresenter[] { };
        [SerializeField] private GameObject _enemiesContainer;

        public void InjectAndInitAll(IObjectResolver _resolver)
        {
            foreach (var enemy in _enemies)
            {
                _resolver.Inject(enemy);
                enemy.Init();
            }

            if (_enemiesContainer != null)
            {
                var childrenEnemy = _enemiesContainer.GetComponentsInChildren<EnemyRootPresenter>();
                foreach (var childEnemy in childrenEnemy)
                {
                    _resolver.Inject(childEnemy);
                    childEnemy.Init();
                }
            }
        }
    }
}