using System;
using System.Threading;

namespace express;

public sealed class AppServer
{
    private readonly Action? _closeAction;

    internal Thread? keepAliveThread { get; }

    public int? port { get; }
    public string? host { get; }
    public string? path { get; }
    public bool listening { get; private set; }

    public AppServer(int? port, string? host, string? path, Action? closeAction = null)
        : this(port, host, path, closeAction, keepAliveThread: null)
    {
    }

    internal AppServer(int? port, string? host, string? path, Action? closeAction, Thread? keepAliveThread)
    {
        this.port = port;
        this.host = host;
        this.path = path;
        _closeAction = closeAction;
        this.keepAliveThread = keepAliveThread;
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
