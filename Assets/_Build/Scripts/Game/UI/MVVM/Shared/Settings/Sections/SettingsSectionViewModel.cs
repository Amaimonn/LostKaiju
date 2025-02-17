using System;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;

public abstract class SettingsSectionViewModel : IViewModel, IDisposable
{
    public ReadOnlyReactiveProperty<bool> IsAnyChanges = new ReactiveProperty<bool>(false);

    protected readonly SettingsModel _model;
    protected readonly CompositeDisposable _disposables;

    public SettingsSectionViewModel(SettingsModel model)
    {
        _model = model;
        _disposables = new();
    }

    public virtual void ApplyChanges()
    {
    }

    public virtual void ResetSettings()
    {
    }
    
    protected virtual void SetCachedSettings()
    {
    }

    public virtual void Dispose() 
    {
    }
}