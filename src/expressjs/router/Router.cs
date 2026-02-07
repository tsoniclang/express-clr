using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace expressjs;

public class Router : RoutingHost<Router>
{
    private readonly List<layer> _layers = new();
    private readonly Dictionary<string, List<ParamHandler>> _paramHandlers = new(StringComparer.Ordinal);

    public Router(RouterOptions? options = null)
    {
        _ = options;
    }

    public override Router all(object path, object callback, params object[] callbacks) => addRoute(null, path, callback, callbacks);
    public override Router checkout(object path, object callback, params object[] callbacks) => addRoute("CHECKOUT", path, callback, callbacks);
    public override Router copy(object path, object callback, params object[] callbacks) => addRoute("COPY", path, callback, callbacks);
    public override Router delete(object path, object callback, params object[] callbacks) => addRoute("DELETE", path, callback, callbacks);
    public override Router get(object path, object callback, params object[] callbacks) => addRoute("GET", path, callback, callbacks);
    public override Router head(object path, object callback, params object[] callbacks) => addRoute("HEAD", path, callback, callbacks);
    public override Router lock_(object path, object callback, params object[] callbacks) => addRoute("LOCK", path, callback, callbacks);
    public override Router merge(object path, object callback, params object[] callbacks) => addRoute("MERGE", path, callback, callbacks);
    public override Router mkactivity(object path, object callback, params object[] callbacks) => addRoute("MKACTIVITY", path, callback, callbacks);
    public override Router mkcol(object path, object callback, params object[] callbacks) => addRoute("MKCOL", path, callback, callbacks);
    public override Router move(object path, object callback, params object[] callbacks) => addRoute("MOVE", path, callback, callbacks);
    public override Router m_search(object path, object callback, params object[] callbacks) => addRoute("M-SEARCH", path, callback, callbacks);
    public override Router notify(object path, object callback, params object[] callbacks) => addRoute("NOTIFY", path, callback, callbacks);
    public override Router options(object path, object callback, params object[] callbacks) => addRoute("OPTIONS", path, callback, callbacks);
    public override Router patch(object path, object callback, params object[] callbacks) => addRoute("PATCH", path, callback, callbacks);
    public override Router post(object path, object callback, params object[] callbacks) => addRoute("POST", path, callback, callbacks);
    public override Router purge(object path, object callback, params object[] callbacks) => addRoute("PURGE", path, callback, callbacks);
    public override Router put(object path, object callback, params object[] callbacks) => addRoute("PUT", path, callback, callbacks);
    public override Router report(object path, object callback, params object[] callbacks) => addRoute("REPORT", path, callback, callbacks);
    public override Router search(object path, object callback, params object[] callbacks) => addRoute("SEARCH", path, callback, callbacks);
    public override Router subscribe(object path, object callback, params object[] callbacks) => addRoute("SUBSCRIBE", path, callback, callbacks);
    public override Router trace(object path, object callback, params object[] callbacks) => addRoute("TRACE", path, callback, callbacks);
    public override Router unlock(object path, object callback, params object[] callbacks) => addRoute("UNLOCK", path, callback, callbacks);
    public override Router unsubscribe(object path, object callback, params object[] callbacks) => addRoute("UNSUBSCRIBE", path, callback, callbacks);

    public override Router method(string method, object path, object callback, params object[] callbacks)
    {
        return addRoute(method?.Trim().ToUpperInvariant(), path, callback, callbacks);
    }

    public override Router param(string name, ParamHandler callback)
    {
        if (!_paramHandlers.TryGetValue(name, out var callbacks))
        {
            callbacks = new List<ParamHandler>();
            _paramHandlers[name] = callbacks;
        }

        callbacks.Add(callback);
        return this;
    }

    public override Route route(object path)
    {
        return new Route(this, path);
    }

    public override Router use(object callback, params object[] callbacks)
    {
        return addMiddleware("/", callback, callbacks);
    }

    public override Router use(object path, object callback, params object[] callbacks)
    {
        return addMiddleware(path, callback, callbacks);
    }

    internal async Task handle(HttpContext context, Application? app = null)
    {
        var request = Request.fromHttpContext(context, app);
        var response = Response.fromHttpContext(context, request);
        request.res = response;
        var processedParams = new HashSet<string>(StringComparer.Ordinal);

        var routePath = request.path;
        var requestMethod = request.method.ToUpperInvariant();
        Exception? error = null;

        foreach (var layer in _layers)
        {
            if (!matchesLayer(layer, routePath, layer.middleware, out var extractedParams))
                continue;

            if (layer.path is string basePath)
                request.baseUrl = basePath == "/" ? string.Empty : normalizePath(basePath);

            if (!layer.middleware && layer.method is { } layerMethod && !string.Equals(layerMethod, requestMethod, StringComparison.OrdinalIgnoreCase))
                continue;

            foreach (var kvp in extractedParams)
                request.@params[kvp.Key] = kvp.Value;

            if (!layer.middleware)
                request.route = new Route(this, layer.path);

            await runParamHandlers(request, response, processedParams).ConfigureAwait(false);

            var control = await invokeHandlers(layer.handlers, request, response, error).ConfigureAwait(false);
            if (control.error is not null)
                error = control.error;

            if (control.ended || response.headersSent)
                return;

            if (string.Equals(control.control, "router", StringComparison.OrdinalIgnoreCase))
                return;

            if (string.Equals(control.control, "route", StringComparison.OrdinalIgnoreCase))
                continue;
        }
    }

    private Router addRoute(string? method, object path, object callback, params object[] callbacks)
    {
        var layer = new layer
        {
            path = path,
            method = method,
            middleware = false
        };
        layer.handlers.AddRange(flattenHandlers(callback, callbacks));
        _layers.Add(layer);
        return this;
    }

    private Router addMiddleware(object path, object callback, params object[] callbacks)
    {
        var allHandlers = flattenHandlers(callback, callbacks);
        foreach (var handler in allHandlers)
        {
            if (handler is Router mountedRouter)
            {
                foreach (var exported in mountedRouter.export(path))
                    _layers.Add(exported);
                continue;
            }

            if (handler is Application mountedApp)
            {
                foreach (var exported in mountedApp.export(path))
                    _layers.Add(exported);
                continue;
            }

            var layer = new layer
            {
                path = path,
                middleware = true
            };
            layer.handlers.Add(handler);
            _layers.Add(layer);
        }

        return this;
    }

    internal List<layer> export(object mountPath)
    {
        var result = new List<layer>();
        foreach (var item in _layers)
        {
            var clone = new layer
            {
                path = combinePath(mountPath, item.path),
                method = item.method,
                middleware = item.middleware
            };
            clone.handlers.AddRange(item.handlers);
            result.Add(clone);
        }

        return result;
    }

    private static object combinePath(object left, object right)
    {
        if (left is not string leftPath)
            return right;
        if (right is not string rightPath)
            return right;

        var lhs = leftPath.TrimEnd('/');
        var rhs = rightPath.TrimStart('/');
        if (string.IsNullOrWhiteSpace(lhs) || lhs == "/")
            return "/" + rhs;
        if (string.IsNullOrWhiteSpace(rhs))
            return lhs;
        return $"{lhs}/{rhs}";
    }

    private static List<object> flattenHandlers(object callback, object[] callbacks)
    {
        var result = new List<object>();
        appendHandler(result, callback);
        foreach (var item in callbacks)
            appendHandler(result, item);
        return result;
    }

    private static void appendHandler(List<object> output, object handler)
    {
        if (handler is null)
            return;

        if (handler is string)
        {
            output.Add(handler);
            return;
        }

        if (handler is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item is null)
                    continue;
                appendHandler(output, item);
            }

            return;
        }

        output.Add(handler);
    }

    private static bool matchesLayer(layer layer, string requestPath, bool middleware, out Dictionary<string, object?> parameters)
    {
        parameters = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        return matchesPathSpec(layer.path, requestPath, middleware, parameters);
    }

    private static bool matchesPathSpec(object? pathSpec, string requestPath, bool middleware, Dictionary<string, object?> parameters)
    {
        if (pathSpec is null)
            return true;

        if (pathSpec is string pathString)
            return matchesStringPath(pathString, requestPath, middleware, parameters);

        if (pathSpec is Regex regex)
            return regex.IsMatch(requestPath);

        if (pathSpec is IEnumerable enumerable && pathSpec is not string)
        {
            foreach (var candidate in enumerable)
            {
                if (matchesPathSpec(candidate, requestPath, middleware, parameters))
                    return true;
            }

            return false;
        }

        return false;
    }

    private static bool matchesStringPath(string pathSpec, string requestPath, bool middleware, Dictionary<string, object?> parameters)
    {
        var normalizedPath = normalizePath(requestPath);
        var normalizedSpec = normalizePath(pathSpec);

        if (normalizedSpec == "/" || string.IsNullOrWhiteSpace(normalizedSpec))
            return true;

        if (normalizedSpec.Contains("{*splat}", StringComparison.Ordinal))
            return normalizedPath.StartsWith(normalizedSpec.Replace("{*splat}", string.Empty), StringComparison.OrdinalIgnoreCase);

        if (normalizedSpec.Contains(':'))
            return matchColonParams(normalizedSpec, normalizedPath, middleware, parameters);

        if (middleware)
            return normalizedPath.Equals(normalizedSpec, StringComparison.OrdinalIgnoreCase) ||
                   normalizedPath.StartsWith(normalizedSpec + "/", StringComparison.OrdinalIgnoreCase);

        return normalizedPath.Equals(normalizedSpec, StringComparison.OrdinalIgnoreCase);
    }

    private static bool matchColonParams(string pattern, string value, bool middleware, Dictionary<string, object?> parameters)
    {
        var patternParts = pattern.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var valueParts = value.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (!middleware && patternParts.Length != valueParts.Length)
            return false;
        if (middleware && patternParts.Length > valueParts.Length)
            return false;

        for (var index = 0; index < patternParts.Length; index++)
        {
            var part = patternParts[index];
            var current = valueParts[index];
            if (part.StartsWith(":", StringComparison.Ordinal))
            {
                parameters[part[1..]] = Uri.UnescapeDataString(current);
                continue;
            }

            if (!string.Equals(part, current, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    private static string normalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "/";

        var normalized = path;
        if (!normalized.StartsWith("/", StringComparison.Ordinal))
            normalized = "/" + normalized;
        if (normalized.Length > 1)
            normalized = normalized.TrimEnd('/');
        return normalized;
    }

    private async Task runParamHandlers(Request request, Response response, HashSet<string> processedParams)
    {
        foreach (var kvp in request.@params)
        {
            var key = $"{kvp.Key}:{kvp.Value}";
            if (!processedParams.Add(key))
                continue;

            if (!_paramHandlers.TryGetValue(kvp.Key, out var callbacks))
                continue;

            foreach (var callback in callbacks)
            {
                await callback(request, response, static _ => Task.CompletedTask, kvp.Value, kvp.Key).ConfigureAwait(false);
            }
        }
    }

    private static async Task<(bool ended, string? control, Exception? error)> invokeHandlers(List<object> handlers, Request request, Response response, Exception? currentError)
    {
        Exception? error = currentError;
        foreach (var handler in handlers)
        {
            if (handler is Delegate callback)
            {
                var parameterCount = callback.Method.GetParameters().Length;
                if (error is null && parameterCount == 4)
                    continue;
                if (error is not null && parameterCount != 4)
                    continue;
                if (error is null && parameterCount < 2)
                    continue;

                var nextCalled = false;
                string? control = null;
                async Task next(string? value)
                {
                    nextCalled = true;
                    control = value;
                    await Task.CompletedTask.ConfigureAwait(false);
                }

                try
                {
                    object? result;
                    if (parameterCount == 4)
                        result = callback.DynamicInvoke(error, request, response, (NextFunction)next);
                    else if (parameterCount == 2)
                        result = callback.DynamicInvoke(request, response);
                    else
                        result = callback.DynamicInvoke(request, response, (NextFunction)next);

                    if (result is Task task)
                        await task.ConfigureAwait(false);

                    if (nextCalled)
                    {
                        if (!string.IsNullOrWhiteSpace(control))
                            return (false, control, null);
                        continue;
                    }

                    return (true, null, null);
                }
                catch (Exception ex)
                {
                    error = ex.InnerException ?? ex;
                    continue;
                }
            }
        }

        return (false, null, error);
    }
}
