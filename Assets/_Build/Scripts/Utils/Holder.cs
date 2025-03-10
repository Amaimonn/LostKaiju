using System;
using System.Collections.Generic;
using System.Linq;
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
                var resolvedKey = _items.Keys.FirstOrDefault(x => resolveType.IsAssignableFrom(x));
                if (resolvedKey != null)
                {
                    return (TKeyType)_items[resolvedKey];
                }
                else
                {
                    Debug.LogError($"Can't resolve {resolveType.Name}");
                    throw new InvalidOperationException();
                }
            }
        }

        public void Register<TKeyType>(TKeyType item) where TKeyType : TElement
        {
            var registeredType = typeof(TKeyType);

            if (_items.ContainsKey(registeredType))
                Debug.Log($"Overriding key {registeredType.Name}");

            _items[registeredType] = item;
        }

        public void Register(Type keyType, TElement item)
        {
            if (!typeof(TElement).IsAssignableFrom(keyType))
            {
                Debug.LogError($"Type '{keyType.Name}' is not a {typeof(TElement)}.");
                return;
            }

            var itemType = item.GetType();

            if (!keyType.IsAssignableFrom(itemType))
            {
                Debug.LogError($"Type '{itemType}' is not a {keyType.Name}.");
                return;
            }

            if (_items.ContainsKey(keyType))
                Debug.Log($"Overriding key {keyType.Name}");

            _items[keyType] = item;
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