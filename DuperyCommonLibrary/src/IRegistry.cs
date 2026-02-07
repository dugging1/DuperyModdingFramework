using System.Collections.Generic;

namespace DMF_Lib;


public interface IRegistry<T>
{
    /// <summary>
    /// Register a new value to the given ID.
    /// </summary>
    void Register(ID id, T val);
    /// <summary>
    /// Retreive the value registered at the given ID.
    /// </summary>
    T Lookup(ID id);
    /// <summary>
    /// Checks for the presence of a value registered with the given ID.
    /// </summary>
    bool ContainsKey(ID ID);
    IEnumerable<KeyValuePair<ID, T>> KeyValue { get; }
}

public interface IBiRegistry<T> : IRegistry<T>
{
    /// <summary>
    /// Retreives the ID registered with the given value.
    /// </summary>
    ID ReverseLookup(T Val);
}
