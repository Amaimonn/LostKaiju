using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostKaiju.Utils
{
    public class Holder<TElement>
    {
        protected Dictionary<Type, TElement> _items = new();

        public virtual TKeyType Resolve<TKeyType>() where TKeyType : TElement
        {
            var resolveType = typeof(TKeyType);
            if (_items.TryGetValue(resolveType, out var item))
            {
                return (TKeyType)item;
            }
            else
            {
                Debug.LogWarning($"Can't resolve {resolveType.Name}");
                return default;
            }
        }

        public void Register<TKeyType>(TKeyType item) where TKeyType : TElement
        {
            var registeredType = typeof(TKeyType);

            if (_items.ContainsKey(registeredType))
                Debug.Log($"Override key {registeredType.Name}");

            _items[registeredType] = item;
        }

        public void Remove<TKeyType>()
        {
            var unregisteredType = typeof(TKeyType);

            if (_items.ContainsKey(unregisteredType))
                _items.Remove(unregisteredType);
            else
                Debug.LogWarning($"There is no key type {unregisteredType.Name} in Holder");
        }
    }
}