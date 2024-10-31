using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayer
{    
    public Rigidbody2D PlayerRigidbody => _rigidbody;
    public Animator PlayerAnimator => _animator;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Flipper _flipper;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private InputModelConfig _inputModelConfig;

    protected CharacterModel CurrentModel => _currentConfig == _inputModelConfig ? _currentModel : SetInputModel(_inputModelConfig);
    
    protected Holder<IEntityFeature> _features = new();
    protected InputModelConfig _currentConfig;
    protected CharacterModel _currentModel;

    // public void Bind(InputModel model)
    // {
    //     // _inputModel = model;
    // }

    private void Awake()
    {
        _features.Register<Flipper>(_flipper);
        _features.Register<GroundCheck>(_groundCheck);
    }

    private void Update()
    {
        CurrentModel.UpdateLogic();
    }

    private void FixedUpdate()
    {
        CurrentModel.FixedUpdateLogic();
    }

    private CharacterModel SetInputModel(InputModelConfig config)
    {
        Debug.Log("New model set up");
        _currentConfig = config;
        _currentModel = config.GetModel();
        _currentModel.Bind(this, _features);
        return _currentModel;
    }
}
