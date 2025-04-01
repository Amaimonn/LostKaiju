using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using R3;

using LostKaiju.Game.GameData.Settings;
using System.Drawing;
using UnityEngine;

namespace LostKaiju.Infrastructure.Managers
{
    public class PostProcessingManager : IDisposable
    {
        private readonly Volume _volume;
        private readonly CompositeDisposable _disposables = new();

        public PostProcessingManager(Volume volume)
        {
            _volume = volume;
        }

        public void BindFromSettings(SettingsModel settingsModel)
        {
            if (_volume == null) 
                return;

            settingsModel.IsPostProcessingEnabled
                .Subscribe(x => _volume.enabled = x)
                .AddTo(_disposables);

            if (_volume.profile.TryGet<Bloom>(out var bloom))
            {
                settingsModel.IsBloomEnabled
                    .Subscribe(x => bloom.active = x)
                    .AddTo(_disposables);
            }

            if (_volume.profile.TryGet<FilmGrain>(out var filmGrain))
            {
                settingsModel.IsFilmGrainEnabled
                    .Subscribe(x => filmGrain.active = x)
                    .AddTo(_disposables);
            }

            if (_volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                var volumeColor = colorAdjustments.colorFilter.value;
                settingsModel.Brightness
                    .Subscribe(x => colorAdjustments.colorFilter.value = volumeColor * Mathf.Pow(2, x / 5f))
                    .AddTo(_disposables);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}