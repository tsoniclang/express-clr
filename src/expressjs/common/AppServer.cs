using System;

namespace expressjs;

public sealed class AppServer
{
    private readonly Action? _closeAction;

    public int? port { get; }
    public string? host { get; }
    public string? path { get; }
    public bool listening { get; private set; }

    public AppServer(int? port, string? host, string? path, Action? closeAction = null)
    {
        this.port = port;
        this.host = host;
        this.path = path;
        _closeAction = closeAction;
        listening = true;
    }

    public void close(Action<Exception?>? callback = null)
    {
        try
        {
            _closeAction?.Invoke();
            listening = false;
            callback?.Invoke(null);
        }
        catch (Exception ex)
        {
            callback?.Invoke(ex);
        }
    }
}
