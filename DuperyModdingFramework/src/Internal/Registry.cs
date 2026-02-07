using System;
using System.Collections.Generic;
using System.Linq;
using DMF_Lib;

namespace DuperyModdingFramework.Internal;

public class Registry<T> : IRegistry<T>
{
    protected Dictionary<ID, T> data;

    public Registry()
    {
        data = new Dictionary<ID, T>();
    }

    public void Register(ID id, T val)
    {
        if (data.ContainsKey(id))
        {
            throw new ArgumentException($"Attempted to register a value with an existing ID: {id}");
        }
        data.Add(id, val);
    }

    public T Lookup(ID id) => data[id];

    public bool ContainsKey(ID ID)
        => data.ContainsKey(ID);

    public IEnumerable<KeyValuePair<ID, T>> KeyValue => data.AsEnumerable();

}

public class IntRegistry : IBiRegistry<int>
{
    protected Dictionary<ID, int> Mapping = new Dictionary<ID, int>();
    protected Dictionary<int, ID> ReverseMapping = new();

    public IEnumerable<KeyValuePair<ID, int>> KeyValue => Mapping;

    public bool ContainsKey(ID ID)
     => Mapping.ContainsKey(ID);

    public int Lookup(ID id) => Mapping[id];

    public void Register(ID id, int val)
    {
        if (Mapping.ContainsKey(id) || ReverseMapping.ContainsKey(val))
        {
            throw new ArgumentException($"Attempted to register a value-ID pair where at least one has already been registered: ({id}) & ({val})");
        }
        Mapping.Add(id, val);
        ReverseMapping.Add(val, id);
    }

    public ID ReverseLookup(int Val) => ReverseMapping[Val];
}

