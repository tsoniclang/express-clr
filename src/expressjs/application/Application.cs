using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace expressjs;

public class Application : Router
{
    private readonly Dictionary<string, object?> _settings = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, TemplateEngine> _engines = new(StringComparer.OrdinalIgnoreCase);

    public event Action<Application>? mount;

    public Dictionary<string, object?> locals { get; } = new(StringComparer.OrdinalIgnoreCase);
    public object mountpath { get; set; } = "/";
    public Router router => this;

    public Application param(string[] names, ParamHandler callback)
    {
        foreach (var name in names)
            base.param(name, callback);
        return this;
    }

    public Application disable(string name)
    {
        _settings[name] = false;
        return this;
    }

    public bool disabled(string name)
    {
        return _settings.TryGetValue(name, out var value) && value is bool b && b == false;
    }

    public Application enable(string name)
    {
        _settings[name] = true;
        return this;
    }

    public bool enabled(string name)
    {
        return _settings.TryGetValue(name, out var value) && value is bool b && b;
    }

    public Application engine(string ext, TemplateEngine callback)
    {
        _engines[ext.TrimStart('.')] = callback;
        return this;
    }

    public object? get(string name)
    {
        _settings.TryGetValue(name, out var value);
        return value;
    }

    public AppServer listen(string path, Action? callback = null)
    {
        var app = buildWebApplication();
        app.Urls.Add($"http://unix:{path}");
        app.Start();
        callback?.Invoke();
        return new AppServer(null, null, path);
    }

    public AppServer listen(int port, Action? callback = null)
    {
        var app = buildWebApplication();
        app.Urls.Add($"http://127.0.0.1:{port}");
        app.Start();
        callback?.Invoke();
        return new AppServer(port, null, null);
    }

    public AppServer listen(int port, string host, Action? callback = null)
    {
        var app = buildWebApplication();
        app.Urls.Add($"http://{host}:{port}");
        app.Start();
        callback?.Invoke();
        return new AppServer(port, host, null);
    }

    public AppServer listen(int port, string host, int backlog, Action? callback = null)
    {
        _ = backlog;
        return listen(port, host, callback);
    }

    public string path()
    {
        if (mountpath is string mountString)
            return mountString;

        if (mountpath is IEnumerable<string> mountCollection)
            return string.Join(",", mountCollection);

        return string.Empty;
    }

    public void render(string view, Action<Exception?, string?> callback)
    {
        if (_engines.TryGetValue(viewExtension(view), out var engine))
        {
            engine(view, locals, callback);
            return;
        }

        callback(null, $"<rendered:{view}>");
    }

    public void render(string view, Dictionary<string, object?> viewLocals, Action<Exception?, string?> callback)
    {
        if (_engines.TryGetValue(viewExtension(view), out var engine))
        {
            engine(view, viewLocals, callback);
            return;
        }

        callback(null, $"<rendered:{view}>");
    }

    public Application set(string name, object? value)
    {
        _settings[name] = value;
        return this;
    }

    public new Application use(object callback, params object[] callbacks)
    {
        base.use(callback, callbacks);
        notifyMount(callback, "/");
        foreach (var next in callbacks)
            notifyMount(next, "/");
        return this;
    }

    public new Application use(object path, object callback, params object[] callbacks)
    {
        base.use(path, callback, callbacks);
        notifyMount(callback, path);
        foreach (var next in callbacks)
            notifyMount(next, path);
        return this;
    }

    internal TemplateEngine? resolveEngine(string view)
    {
        _engines.TryGetValue(viewExtension(view), out var engine);
        return engine;
    }

    private static string viewExtension(string view)
    {
        var dot = view.LastIndexOf('.');
        if (dot < 0 || dot == view.Length - 1)
            return string.Empty;
        return view[(dot + 1)..];
    }

    private WebApplication buildWebApplication()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.Run(async context =>
        {
            await handle(context, this).ConfigureAwait(false);
            if (!context.Response.HasStarted && context.Response.StatusCode == StatusCodes.Status200OK)
                context.Response.StatusCode = StatusCodes.Status404NotFound;
        });

        return app;
    }

    private void notifyMount(object candidate, object mountedAt)
    {
        if (candidate is not Application child)
            return;

        child.mountpath = mountedAt;
        child.mount?.Invoke(this);
    }
}
