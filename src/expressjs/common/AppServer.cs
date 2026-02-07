using System;

namespace expressjs;

public sealed class AppServer
{
    public int? port { get; }
    public string? host { get; }
    public string? path { get; }
    public bool listening { get; private set; }

    public AppServer(int? port, string? host, string? path)
    {
        this.port = port;
        this.host = host;
        this.path = path;
        listening = true;
    }

    public void close(Action<Exception?>? callback = null)
    {
        listening = false;
        callback?.Invoke(null);
    }
}
