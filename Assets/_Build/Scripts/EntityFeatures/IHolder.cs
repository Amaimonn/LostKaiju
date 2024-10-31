using System;
using System.Collections.Generic;

public class Holder<T>
{
    protected Dictionary<Type, T> _items = new();

    public virtual TT Resolve<TT>() where TT : T
    {
        var item = _items[typeof(TT)];

        if (item == null)
            return default;

        return (TT) _items[typeof(TT)];
    }

    public void Register<TT> (TT item) where TT : T
    {
        _items[typeof(TT)] = item;
    }
}