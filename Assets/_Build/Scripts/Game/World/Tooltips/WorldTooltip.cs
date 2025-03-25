using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LostKaiju.Game.World.Player.Views;

[RequireComponent(typeof(Collider2D))]
public class WorldTooltip : MonoBehaviour
{
    [SerializeField] private GameObject _activeHolder;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeInDuration = 0.3f;
    [SerializeField, Min(0.01f)] private float _fadeOutDuration = 0.2f;
    
    private readonly HashSet<IPlayerHero> _enteredHeroes = new();
    private Coroutine _currentFadeCoroutine;
    private float _currentAlpha = 0;

    private void Awake()
    {
        _activeHolder.SetActive(false);
        _canvasGroup.alpha = _currentAlpha;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IPlayerHero>(out var hero))
        {
            if (_enteredHeroes.Count == 0)
            {
                if (_currentFadeCoroutine != null)
                {
                    StopCoroutine(_currentFadeCoroutine);
                }
                
                _activeHolder.SetActive(true);
                _currentFadeCoroutine = StartCoroutine(Fade(_currentAlpha, 1f, _fadeInDuration));
            }
            
            _enteredHeroes.Add(hero);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IPlayerHero>(out var hero))
        {
            _enteredHeroes.Remove(hero);
            
            if (_enteredHeroes.Count == 0)
            {
                if (_currentFadeCoroutine != null)
                {
                    StopCoroutine(_currentFadeCoroutine);
                }
                
                _currentFadeCoroutine = StartCoroutine(Fade(_currentAlpha, 0f, _fadeOutDuration, 
                    () => _activeHolder.SetActive(false)));
            }
        }
    }

    private IEnumerator Fade(float from, float to, float duration, Action onFadeCompleted = null)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = _currentAlpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        
        _canvasGroup.alpha = _currentAlpha = to;
        onFadeCompleted?.Invoke();
        _currentFadeCoroutine = null;
    }
}