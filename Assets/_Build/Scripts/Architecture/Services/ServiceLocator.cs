using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    public static ServiceLocator Current => _instance ??= new ServiceLocator();
    private static ServiceLocator _instance;
    private readonly Dictionary<string, IService> _services = new();

    public void Register<TService>(TService service) where TService : IService
    {
        string key  = typeof(TService).Name;

        if(_services.ContainsKey(key))
        {
            Debug.LogError("Service already registered:  " + key);
            throw new InvalidOperationException();
        }

        _services.Add(key, service);
    }

    public void Unregister<TService>() where TService  : IService
    {
        string key  = typeof(TService).Name;

        if(!_services.ContainsKey(key))
        {
            Debug.LogError("Service for unregistering not found: " + key);
            throw new InvalidOperationException();
        }

        _services.Remove(key);
    }

    public TService Get<TService>() where TService : IService
    {
        string key = typeof(TService).Name;

        if(!_services.ContainsKey(key))
        {
            Debug.LogError("Service not found: " + key);
            throw new InvalidOperationException();
        }

        return (TService)_services[key];
    }
}
