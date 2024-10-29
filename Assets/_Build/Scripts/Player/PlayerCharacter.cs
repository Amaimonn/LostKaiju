using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayer, IHolder<IEntityFeature>
{    
    public Dictionary<Type, IEntityFeature> Items => _features;
    public GameObject Holder => gameObject;
    public Rigidbody2D PlayerRigidbody => _rigidbody;
    public Animator PlayerAnimator => _animator;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Flipper _flipper;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private InputModelConfig _inputModelConfig;

    protected InputModel CurrentModel => _currentConfig == _inputModelConfig ? _currentModel : SetInputModel(_inputModelConfig);
    
    protected Dictionary<Type, IEntityFeature> _features = new();
    protected InputModelConfig _currentConfig;
    protected InputModel _currentModel;

    // public void Bind(InputModel model)
    // {
    //     // _inputModel = model;
    // }

    private void Awake()
    {
        _features.Add(typeof(Flipper), _flipper);
    }

    private void Update()
    {
        CurrentModel.UpdateLogic();
    }

    private void FixedUpdate()
    {
        CurrentModel.FixedUpdateLogic();
    }

    private InputModel SetInputModel(InputModelConfig config)
    {
        Debug.Log("New model set up");
        _currentConfig = config;
        _currentModel = config.GetModel();
        _currentModel.Bind(this, _groundCheck, _flipper);
        return _currentModel;
    }
}
