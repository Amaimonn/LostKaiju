using System;
using System.Collections.Generic;
using LostKaiju.Game.Constants;
using UnityEngine;

namespace LostKaiju.Game.UI.CustomElements
{
    public class HeroPreviewSelector
    {
        private readonly Dictionary<string, GameObject> _heroPreviewCache = new();

        public GameObject GetPreviewById(string heroId)
        {
            if (_heroPreviewCache.TryGetValue(heroId, out var cachedPreview) && cachedPreview != null)
                return cachedPreview;
            else
                return CreateById(heroId);
        }

        private GameObject CreateById(string heroId)
        {
            var heroPreviewPrefab = Resources.Load<GameObject>($"{Paths.HERO_PREVIEWS}/{heroId}");
            var heroPreview = UnityEngine.Object.Instantiate(heroPreviewPrefab);
            _heroPreviewCache[heroId] = heroPreview;
            
            return heroPreview;
        }

        public void ClearExceptOne(string id)
        {
            if (_heroPreviewCache.TryGetValue(id, out var savedPreview))
            {
                _heroPreviewCache.Remove(id);

                foreach (var preview in _heroPreviewCache.Values)
                {
                    if (preview != null)
                        UnityEngine.Object.Destroy(preview);
                }

                _heroPreviewCache.Clear();
                _heroPreviewCache[id] = savedPreview;
            }
            else
            {
                Debug.LogError($"There is no hero preview in the cache with id {id}");
            }
        }
    }
}