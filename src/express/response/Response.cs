using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace express;

public class Response
{
    private readonly HttpContext? _context;
    private readonly Dictionary<string, string> _headers = new(StringComparer.OrdinalIgnoreCase);
    private readonly FileExtensionContentTypeProvider _contentTypes = new();

    public Request? req { get; set; }
    public Dictionary<string, object?> locals { get; } = new(StringComparer.OrdinalIgnoreCase);
    public int statusCode { get; private set; } = StatusCodes.Status200OK;
    public bool headersSent { get; private set; }
    public Application? app => req?.app;

    public Response()
    {
    }

    private Response(HttpContext context, Request request)
    {
        _context = context;
        req = request;
        statusCode = context.Response.StatusCode;
    }

    internal static Response fromHttpContext(HttpContext context, Request request)
    {
        return new Response(context, request);
    }

    public Response append(string field, string value)
    {
        if (_headers.TryGetValue(field, out var existing))
            _headers[field] = $"{existing}, {value}";
        else
            _headers[field] = value;

        if (_context is not null)
            _context.Response.Headers[field] = _headers[field];

        return this;
    }

    public Response append(string field, IEnumerable<string> values)
    {
        foreach (var value in values)
            append(field, value);
        return this;
    }

    public Response attachment(string? filename = null)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return set("Content-Disposition", "attachment");

        type(filename);
        return set("Content-Disposition", $"attachment; filename=\"{Path.GetFileName(filename)}\"");
    }

    public Response cookie(string name, object? value, CookieOptions? options = null)
    {
        var payload = value is string s ? s : serializeObject(value);
        if (options?.encode is { } encoder)
            payload = encoder(payload);

        var cookie = $"{name}={payload}; Path={options?.path ?? "/"}";
        append("Set-Cookie", cookie);
        return this;
    }

    public Response clearCookie(string name, CookieOptions? options = null)
    {
        var reset = options ?? new CookieOptions();
        reset.expires = DateTime.UnixEpoch;
        return cookie(name, string.Empty, reset);
    }

    public Response download(string path, string? filename = null, DownloadOptions? options = null, Action<Exception?>? fn = null)
    {
        try
        {
            attachment(filename ?? Path.GetFileName(path));
            sendFile(path, options is null ? null : new SendFileOptions
            {
                maxAge = options.maxAge,
                root = options.root,
                lastModified = options.lastModified,
                headers = options.headers,
                dotfiles = options.dotfiles,
                acceptRanges = options.acceptRanges,
                cacheControl = options.cacheControl,
                immutable = options.immutable
            });
            fn?.Invoke(null);
        }
        catch (Exception ex)
        {
            fn?.Invoke(ex);
        }

        return this;
    }

    public Response end(object? data = null, string? encoding = null, Action? callback = null)
    {
        _ = encoding;
        if (data is not null)
            send(data);

        headersSent = true;
        if (_context is not null)
            _context.Response.StatusCode = statusCode;
        callback?.Invoke();
        return this;
    }

    public Response format(Dictionary<string, Action> handlers)
    {
        if (handlers.Count == 0)
            return this;

        if (handlers.TryGetValue("default", out var fallback))
        {
            fallback();
            return this;
        }

        handlers.First().Value();
        return this;
    }

    public string? get(string field)
    {
        if (_headers.TryGetValue(field, out var value))
            return value;

        if (_context is not null && _context.Response.Headers.TryGetValue(field, out var header))
            return header.ToString();

        return null;
    }

    public Response json(object? body = null)
    {
        type("application/json");
        return send(body is string s ? s : serializeObject(body));
    }

    public Response jsonp(object? body = null)
    {
        var callbackName = app?.get("jsonp callback name") as string ?? "callback";
        var payload = body is string s ? s : serializeObject(body);
        type("application/javascript");
        return send($"{callbackName}({payload})");
    }

    public Response links(Dictionary<string, string> links)
    {
        var joined = string.Join(", ", links.Select(kvp => $"<{kvp.Value}>; rel=\"{kvp.Key}\""));
        return set("Link", joined);
    }

    public Response location(string path)
    {
        return set("Location", path);
    }

    public Response redirect(string path) => redirect(StatusCodes.Status302Found, path);

    public Response redirect(int status, string path)
    {
        statusCode = status;
        location(path);
        headersSent = true;
        if (_context is not null)
        {
            _context.Response.StatusCode = status;
            _context.Response.Headers.Location = path;
        }

        return this;
    }

    public Response render(string view)
    {
        if (app?.resolveEngine(view) is { } engine)
        {
            engine(view, locals, (_, html) => send(html ?? string.Empty));
            return this;
        }

        return send($"<rendered:{view}>");
    }

    public Response render(string view, Dictionary<string, object?> viewLocals)
    {
        if (app?.resolveEngine(view) is { } engine)
        {
            engine(view, viewLocals, (_, html) => send(html ?? string.Empty));
            return this;
        }

        return send($"<rendered:{view}>");
    }

    public Response render(string view, Action<Exception?, string?> callback)
    {
        if (app?.resolveEngine(view) is { } engine)
        {
            engine(view, locals, callback);
            return this;
        }

        callback(null, $"<rendered:{view}>");
        return this;
    }

    public Response render(string view, Dictionary<string, object?> viewLocals, Action<Exception?, string?> callback)
    {
        if (app?.resolveEngine(view) is { } engine)
        {
            engine(view, viewLocals, callback);
            return this;
        }

        callback(null, $"<rendered:{view}>");
        return this;
    }

    public Response send(object? body = null)
    {
        string payload;
        byte[]? binaryPayload = null;
        if (body is null)
        {
            payload = string.Empty;
        }
        else if (body is string text)
        {
            payload = text;
        }
        else if (body is byte[] bytes)
        {
            payload = string.Empty;
            binaryPayload = bytes;
            if (get("Content-Type") is null)
                type("application/octet-stream");
        }
        else
        {
            if (get("Content-Type") is null)
                type("application/json");
            payload = serializeObject(body);
        }

        if (_context is not null)
        {
            _context.Response.StatusCode = statusCode;
            foreach (var header in _headers)
                _context.Response.Headers[header.Key] = header.Value;
            if (binaryPayload is not null)
                _context.Response.Body.Write(binaryPayload, 0, binaryPayload.Length);
            else
                _context.Response.WriteAsync(payload).GetAwaiter().GetResult();
        }

        headersSent = true;
        return this;
    }

    public Response sendFile(string path, SendFileOptions? options = null, Action<Exception?>? fn = null)
    {
        try
        {
            var resolved = path;
            if (!Path.IsPathRooted(resolved) && !string.IsNullOrWhiteSpace(options?.root))
                resolved = Path.GetFullPath(Path.Combine(options.root!, path));

            if (!File.Exists(resolved))
                throw new FileNotFoundException("File not found", resolved);

            if (_context is not null)
            {
                _context.Response.StatusCode = statusCode;
                if (_contentTypes.TryGetContentType(resolved, out var contentType))
                    _context.Response.ContentType = contentType;
                _context.Response.SendFileAsync(resolved).GetAwaiter().GetResult();
            }

            headersSent = true;
            fn?.Invoke(null);
        }
        catch (Exception ex)
        {
            fn?.Invoke(ex);
        }

        return this;
    }

    public Response sendStatus(int code)
    {
        status(code);
        return send(code.ToString());
    }

    public Response set(string field, object? value)
    {
        _headers[field] = value?.ToString() ?? string.Empty;
        if (_context is not null)
            _context.Response.Headers[field] = _headers[field];
        return this;
    }

    public Response set(Dictionary<string, string> fields)
    {
        foreach (var field in fields)
            set(field.Key, field.Value);
        return this;
    }

    public Response header(string field, object? value) => set(field, value);

    public Response status(int code)
    {
        statusCode = code;
        if (_context is not null)
            _context.Response.StatusCode = code;
        return this;
    }

    public Response type(string type)
    {
        var contentType = type;
        if (!type.Contains("/", StringComparison.Ordinal))
        {
            if (!type.StartsWith(".", StringComparison.Ordinal))
                type = "." + type;
            _contentTypes.TryGetContentType("x" + type, out contentType);
        }

        return set("Content-Type", contentType ?? "application/octet-stream");
    }

    public Response contentType(string type) => this.type(type);

    public Response vary(string field)
    {
        if (_headers.TryGetValue("Vary", out var value) && !string.IsNullOrWhiteSpace(value))
            _headers["Vary"] = $"{value}, {field}";
        else
            _headers["Vary"] = field;

        if (_context is not null)
            _context.Response.Headers["Vary"] = _headers["Vary"];

        return this;
    }

    private static string serializeObject(object? body)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        writeJsonValue(writer, body, new HashSet<object>(ReferenceEqualityComparer.Instance), depth: 0);
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void writeJsonValue(Utf8JsonWriter writer, object? value, HashSet<object> visited, int depth)
    {
        if (depth > 64)
            throw new InvalidOperationException("JSON value nesting exceeded the supported depth.");

        if (value is not null && value is not string && !value.GetType().IsValueType)
        {
            if (!visited.Add(value))
                throw new InvalidOperationException("Circular reference detected while serializing JSON.");
        }

        try
        {
        switch (value)
        {
            case null:
                writer.WriteNullValue();
                return;
            case JsonElement element:
                element.WriteTo(writer);
                return;
            case JsonDocument document:
                document.RootElement.WriteTo(writer);
                return;
            case string text:
                writer.WriteStringValue(text);
                return;
            case bool boolean:
                writer.WriteBooleanValue(boolean);
                return;
            case byte number:
                writer.WriteNumberValue(number);
                return;
            case sbyte number:
                writer.WriteNumberValue(number);
                return;
            case short number:
                writer.WriteNumberValue(number);
                return;
            case ushort number:
                writer.WriteNumberValue(number);
                return;
            case int number:
                writer.WriteNumberValue(number);
                return;
            case uint number:
                writer.WriteNumberValue(number);
                return;
            case long number:
                writer.WriteNumberValue(number);
                return;
            case ulong number:
                writer.WriteNumberValue(number);
                return;
            case float number:
                writer.WriteNumberValue(number);
                return;
            case double number:
                writer.WriteNumberValue(number);
                return;
            case decimal number:
                writer.WriteNumberValue(number);
                return;
            case Dictionary<string, object?> objectMap:
                writer.WriteStartObject();
                foreach (var kvp in objectMap)
                {
                    writer.WritePropertyName(kvp.Key);
                    writeJsonValue(writer, kvp.Value, visited, depth + 1);
                }
                writer.WriteEndObject();
                return;
            case IDictionary<string, string> stringMap:
                writer.WriteStartObject();
                foreach (var kvp in stringMap)
                {
                    writer.WritePropertyName(kvp.Key);
                    writer.WriteStringValue(kvp.Value);
                }
                writer.WriteEndObject();
                return;
            case IEnumerable enumerable when value is not string:
                writer.WriteStartArray();
                foreach (var item in enumerable)
                    writeJsonValue(writer, item, visited, depth + 1);
                writer.WriteEndArray();
                return;
            default:
                if (value is not null && tryWriteObject(writer, value, visited, depth + 1))
                    return;

                writer.WriteStringValue(value?.ToString());
                return;
        }
        }
        finally
        {
            if (value is not null && value is not string && !value.GetType().IsValueType)
                visited.Remove(value);
        }
    }

    private static bool tryWriteObject(Utf8JsonWriter writer, object value, HashSet<object> visited, int depth)
    {
#pragma warning disable IL2075
        var props = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
#pragma warning restore IL2075
        if (props.Length == 0)
            return false;

        writer.WriteStartObject();

        foreach (var prop in props)
        {
            if (prop.GetIndexParameters().Length != 0)
                continue;
            if (!prop.CanRead)
                continue;

            writer.WritePropertyName(prop.Name);

            object? propValue;
            try
            {
                propValue = prop.GetValue(value);
            }
            catch
            {
                propValue = null;
            }

            writeJsonValue(writer, propValue, visited, depth + 1);
        }

        writer.WriteEndObject();
        return true;
    }
}
