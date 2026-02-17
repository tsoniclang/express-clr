using System;
using System.Collections.Generic;

namespace express;

public sealed class Params
{
    private readonly Dictionary<string, object?> _values = new(StringComparer.OrdinalIgnoreCase);

    public string? this[string key]
    {
        get
        {
            if (_values.TryGetValue(key, out var value))
                return value?.ToString();
            return null;
        }
    }

    internal void Set(string key, object? value)
    {
        _values[key] = value;
    }

    internal IEnumerable<KeyValuePair<string, object?>> Entries()
    {
        return _values;
    }
}
