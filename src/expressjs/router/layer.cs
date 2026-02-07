using System.Collections.Generic;

namespace expressjs;

internal sealed class layer
{
    public object path { get; set; } = "/";
    public string? method { get; set; }
    public bool middleware { get; set; }
    public List<object> handlers { get; } = new();
}
