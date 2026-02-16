using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace express;

public class Request
{
    private readonly HttpContext? _context;
    private readonly HashSet<string> _acceptedLanguages = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _headers = new(StringComparer.OrdinalIgnoreCase);

    public Application? app { get; set; }
    public string baseUrl { get; set; } = string.Empty;
    public object? body { get; set; }
    public Dictionary<string, string> cookies { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public bool fresh { get; set; }
    public string host { get; set; } = string.Empty;
    public string hostname { get; set; } = string.Empty;
    public string ip { get; set; } = string.Empty;
    public List<string> ips { get; set; } = new();
    public string method { get; set; } = "GET";
    public string originalUrl { get; set; } = "/";
    public Dictionary<string, object?> @params { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public string path { get; set; } = "/";
    public string protocol { get; set; } = "http";
    public Dictionary<string, object?> query { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Response? res { get; set; }
    public Route? route { get; set; }
    public bool signed { get; set; }
    public Dictionary<string, string> signedCookies { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public List<string> subdomains { get; set; } = new();
    public bool xhr { get; set; }

    public bool secure => string.Equals(protocol, "https", StringComparison.OrdinalIgnoreCase);
    public bool stale => !fresh;

    public Request()
    {
    }

    private Request(HttpContext context, Application? application)
    {
        _context = context;
        app = application;
        method = context.Request.Method;
        path = context.Request.Path.HasValue ? context.Request.Path.Value! : "/";
        baseUrl = string.Empty;
        originalUrl = path + context.Request.QueryString.Value;
        protocol = context.Request.Scheme;
        host = context.Request.Host.ToString();
        hostname = context.Request.Host.Host;
        ip = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        xhr = string.Equals(context.Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
        fresh = false;

        foreach (var header in context.Request.Headers)
            _headers[header.Key] = header.Value.ToString();

        foreach (var cookie in context.Request.Cookies)
            cookies[cookie.Key] = cookie.Value;

        foreach (var item in context.Request.Query)
            query[item.Key] = item.Value.Count == 1 ? item.Value[0] : item.Value.ToArray();

        if (!string.IsNullOrWhiteSpace(hostname))
        {
            var parts = hostname.Split('.', StringSplitOptions.RemoveEmptyEntries);
            var offset = application?.get("subdomain offset") as int? ?? 2;
            if (parts.Length > offset)
            {
                for (var index = parts.Length - offset - 1; index >= 0; index--)
                    subdomains.Add(parts[index]);
            }
        }
    }

    internal static Request fromHttpContext(HttpContext context, Application? application)
    {
        return new Request(context, application);
    }

    internal HttpContext? context => _context;

    public object? accepts(params string[] types)
    {
        if (types.Length == 0)
            return false;
        return types[0];
    }

    public object? acceptsCharsets(params string[] charsets)
    {
        if (charsets.Length == 0)
            return false;
        return charsets[0];
    }

    public object? acceptsEncodings(params string[] encodings)
    {
        if (encodings.Length == 0)
            return false;
        return encodings[0];
    }

    public object acceptsLanguages(params string[] languages)
    {
        if (languages.Length == 0)
        {
            var raw = get("Accept-Language");
            if (string.IsNullOrWhiteSpace(raw))
                return Array.Empty<string>();

            return raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split(';')[0].Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToArray();
        }

        return languages[0];
    }

    public string? get(string field)
    {
        if (_headers.TryGetValue(field, out var value))
            return value;

        if (_context is null)
            return null;

        if (_context.Request.Headers.TryGetValue(field, out var header))
            return header.ToString();

        return null;
    }

    public string? header(string field) => get(field);

    public string? param(string name)
    {
        if (@params.TryGetValue(name, out var value))
            return value?.ToString();

        return null;
    }

    public object? @is(params string[] types)
    {
        if (types.Length == 0)
            return null;
        if (body is null)
            return null;
        return types[0];
    }

    public object range(long size, RangeOptions? options = null)
    {
        _ = options;
        if (size <= 0)
            return -1;

        var result = new RangeResult();
        result.ranges.Add(new ByteRange { start = 0, end = size - 1 });
        return result;
    }

    public void setHeader(string name, string value)
    {
        _headers[name] = value;
    }
}
