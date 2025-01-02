using System;
using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Combat.AttackSystem
{
    [Serializable]
    public class AttackProvider
    {
        [SerializeField] private AttackPathSO[] _attackPaths;

        public IAttackPath GetPath()
        {
            if (_attackPaths == null || _attackPaths.Length == 0)
                return null;

            return _attackPaths[0];
        }
    }
}