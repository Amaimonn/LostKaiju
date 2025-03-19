using System;
using UnityEngine;
using VContainer;

namespace LostKaiju.Game.World.Enemy
{
    [Serializable]
    public class EnemyInjector
    {
        [SerializeField] private EnemyRootPresenter[] _enemies = new EnemyRootPresenter[] { };

        public void InjectAndInitAll(IObjectResolver _resolver)
        {
            foreach (var enemy in _enemies)
            {
                _resolver.Inject(enemy);
                enemy.Init();
            }
        }
    }
}