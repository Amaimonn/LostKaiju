using System;
using System.Collections.Generic;
using LostKaiju.Game.Constants;
using UnityEngine;

namespace LostKaiju.Game.UI.CustomElements
{
    public class HeroPreviewSelector
    {
        private readonly Dictionary<string, GameObject> _heroPreviewContainersCache = new();

        public GameObject GetPreviewById(string heroId)
        {
            if (_heroPreviewContainersCache.TryGetValue(heroId, out var cachedPreview) && cachedPreview != null)
                return cachedPreview;
            else
                return CreateById(heroId);
        }

        private GameObject CreateById(string heroId)
        {
            var heroPreviewPrefab = Resources.Load<GameObject>($"{Paths.HERO_PREVIEWS}/{heroId}");
            var heroPreview = UnityEngine.Object.Instantiate(heroPreviewPrefab);
            var previewParent = new GameObject($"{heroPreview.name} Container");
            var parentRectTransform = previewParent.AddComponent<RectTransform>();
            heroPreview.transform.SetParent(parentRectTransform, false);
            _heroPreviewContainersCache[heroId] = previewParent;
            
            return previewParent;
        }

        public void ClearExceptOne(string id)
        {
            if (_heroPreviewContainersCache.TryGetValue(id, out var savedPreview))
            {
                _heroPreviewContainersCache.Remove(id);

                foreach (var preview in _heroPreviewContainersCache.Values)
                {
                    if (preview != null)
                        UnityEngine.Object.Destroy(preview);
                }

                _heroPreviewContainersCache.Clear();
                _heroPreviewContainersCache[id] = savedPreview;
            }
            else
            {
                Debug.LogError($"There is no hero preview in the cache with id {id}");
            }
        }
    }
}