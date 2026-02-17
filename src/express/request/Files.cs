using System;
using System.Collections.Generic;
using System.Linq;

namespace express;

public sealed class Files
{
    private readonly Dictionary<string, List<UploadedFile>> _files = new(StringComparer.OrdinalIgnoreCase);

    public UploadedFile[]? this[string field]
    {
        get
        {
            if (_files.TryGetValue(field, out var list))
                return list.ToArray();
            return null;
        }
    }

    internal void Clear()
    {
        _files.Clear();
    }

    internal void Add(UploadedFile file)
    {
        Add(file.fieldname, file);
    }

    internal void Add(string field, UploadedFile file)
    {
        if (!_files.TryGetValue(field, out var list))
        {
            list = new List<UploadedFile>();
            _files[field] = list;
        }

        list.Add(file);
    }

    internal IEnumerable<KeyValuePair<string, UploadedFile[]>> Entries()
    {
        foreach (var kvp in _files)
            yield return new KeyValuePair<string, UploadedFile[]>(kvp.Key, kvp.Value.ToArray());
    }
}

