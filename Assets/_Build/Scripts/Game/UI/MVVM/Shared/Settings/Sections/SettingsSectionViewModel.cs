using System;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;

public abstract class SettingsSectionViewModel : IViewModel, IDisposable
{
    public ReadOnlyReactiveProperty<bool> IsAnyChanges = new ReactiveProperty<bool>(false);

    protected readonly SettingsModel _model;
    protected CompositeDisposable _disposables;

    public SettingsSectionViewModel(SettingsModel model)
    {
        _model = model;
        _disposables = new();
    }

    public virtual void ApplyChanges()
    {
    }

    public virtual void CancelChanges()
    {
    }

    protected virtual void CacheSettings()
    {
    }

    public virtual void Dispose()
    {
        if (_disposables != null)
        {
            _disposables.Dispose();
            _disposables = null;
        }
    }
}