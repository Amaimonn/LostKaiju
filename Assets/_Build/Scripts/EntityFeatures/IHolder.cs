using System;
using System.Collections.Generic;
using UnityEngine;

public interface IHolder<T>
{
    public Dictionary<Type, T> Items { get; }
    public GameObject Holder { get; }
}
