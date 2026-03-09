using System;
using Tsonic.JSRuntime;

namespace express;

public sealed class AppServer
{
    private readonly Action? _closeAction;
    private readonly int? _port;

    public double? port => js_interop.fromInt32(_port);
    public string? host { get; }
    public string? path { get; }
    public bool listening { get; private set; }

    internal AppServer(int? port, string? host, string? path, Action? closeAction = null)
    {
        _port = port;
        this.host = host;
        this.path = path;
        _closeAction = closeAction;
        listening = true;
    }

    public void close(Action<Error?>? callback = null)
    {
        if (!listening)
        {
            callback?.Invoke(null);
            return;
        }

        try
        {
            _closeAction?.Invoke();
            listening = false;
            callback?.Invoke(null);
        }
        catch (Exception ex)
        {
            callback?.Invoke(js_interop.fromException(ex));
        }
    }
}
