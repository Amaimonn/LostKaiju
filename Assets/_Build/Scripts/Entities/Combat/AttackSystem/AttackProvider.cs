using System;
using LostKaiju.Configs;
using UnityEngine;

namespace LostKaiju.Entities.Combat.AttackSystem
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