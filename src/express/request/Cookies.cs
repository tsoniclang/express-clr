using System;
using System.Collections.Generic;

namespace express;

public sealed class Cookies
{
    private readonly Dictionary<string, string> _values = new(StringComparer.OrdinalIgnoreCase);

    public string? this[string key]
    {
        get
        {
            if (_values.TryGetValue(key, out var value))
                return value;
            return null;
        }
    }

    internal void Set(string key, string value)
    {
        _values[key] = value;
    }

    internal bool Remove(string key)
    {
        return _values.Remove(key);
    }

    internal void Clear()
    {
        _values.Clear();
    }

    internal IEnumerable<KeyValuePair<string, string>> Entries()
    {
        return _values;
    }
}

