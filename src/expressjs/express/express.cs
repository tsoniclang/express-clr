using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace expressjs;

public static class express
{
    public static Application create() => new();
    public static Application application() => new();
    public static Application app() => new();

    public static RequestHandler json(JsonOptions? options = null)
    {
        return async (req, _, next) =>
        {
            if (!matchesType(req, options?.type, "application/json"))
            {
                await next(null).ConfigureAwait(false);
                return;
            }

            var body = await readBody(req).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(body))
            {
                req.body = null;
                await next(null).ConfigureAwait(false);
                return;
            }

            req.body = parseJsonBody(body);
            await next(null).ConfigureAwait(false);
        };
    }

    public static RequestHandler raw(RawOptions? options = null)
    {
        return async (req, _, next) =>
        {
            if (!matchesType(req, options?.type, "application/octet-stream"))
            {
                await next(null).ConfigureAwait(false);
                return;
            }

            var bytes = await readBodyBytes(req).ConfigureAwait(false);
            req.body = bytes;
            await next(null).ConfigureAwait(false);
        };
    }

    public static Router Router(RouterOptions? options = null)
    {
        return new Router(options);
    }

    public static RequestHandler @static(string root, StaticOptions? options = null)
    {
        return async (req, res, next) =>
        {
            var relative = req.path.TrimStart('/');
            if (string.IsNullOrWhiteSpace(relative) && options?.index is string indexFile)
                relative = indexFile;

            var resolved = Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(resolved))
            {
                res.sendFile(resolved, new SendFileOptions
                {
                    acceptRanges = options?.acceptRanges ?? true,
                    cacheControl = options?.cacheControl ?? true,
                    dotfiles = options?.dotfiles ?? "ignore",
                    immutable = options?.immutable ?? false,
                    lastModified = options?.lastModified ?? true,
                    maxAge = options?.maxAge ?? 0
                });
                return;
            }

            await next(null).ConfigureAwait(false);
        };
    }

    public static RequestHandler text(TextOptions? options = null)
    {
        return async (req, _, next) =>
        {
            if (!matchesType(req, options?.type, "text/plain"))
            {
                await next(null).ConfigureAwait(false);
                return;
            }

            req.body = await readBody(req).ConfigureAwait(false);
            await next(null).ConfigureAwait(false);
        };
    }

    public static RequestHandler urlencoded(UrlEncodedOptions? options = null)
    {
        return async (req, _, next) =>
        {
            if (!matchesType(req, options?.type, "application/x-www-form-urlencoded"))
            {
                await next(null).ConfigureAwait(false);
                return;
            }

            var body = await readBody(req).ConfigureAwait(false);
            var parsed = QueryHelpers.ParseQuery(body);
            var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in parsed)
            {
                if (item.Value.Count <= 1)
                    result[item.Key] = item.Value.ToString();
                else
                    result[item.Key] = item.Value.ToArray();
            }

            req.body = result;
            await next(null).ConfigureAwait(false);
        };
    }

    private static bool matchesType(Request req, object? configuredType, string defaultType)
    {
        var contentType = req.get("Content-Type") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(contentType))
            return false;

        if (configuredType is null)
            return contentType.Contains(defaultType, StringComparison.OrdinalIgnoreCase);

        if (configuredType is string one)
            return contentType.Contains(one, StringComparison.OrdinalIgnoreCase);

        if (configuredType is IEnumerable<string> many)
        {
            foreach (var item in many)
            {
                if (contentType.Contains(item, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }

        if (configuredType is MediaTypeMatcher matcher)
            return matcher(req);

        return contentType.Contains(defaultType, StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<string> readBody(Request req)
    {
        var bytes = await readBodyBytes(req).ConfigureAwait(false);
        return Encoding.UTF8.GetString(bytes);
    }

    private static async Task<byte[]> readBodyBytes(Request req)
    {
        if (req.context is null)
            return Array.Empty<byte>();

        using var buffer = new MemoryStream();
        await req.context.Request.Body.CopyToAsync(buffer).ConfigureAwait(false);
        if (req.context.Request.Body.CanSeek)
            req.context.Request.Body.Position = 0;
        return buffer.ToArray();
    }

    private static object? parseJsonBody(string body)
    {
        using var document = JsonDocument.Parse(body);
        return mapJsonElement(document.RootElement);
    }

    private static object? mapJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
            {
                var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                foreach (var property in element.EnumerateObject())
                    result[property.Name] = mapJsonElement(property.Value);
                return result;
            }
            case JsonValueKind.Array:
            {
                var result = new List<object?>();
                foreach (var item in element.EnumerateArray())
                    result.Add(mapJsonElement(item));
                return result;
            }
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                if (element.TryGetInt64(out var longValue))
                    return longValue;
                if (element.TryGetDecimal(out var decimalValue))
                    return decimalValue;
                return element.GetDouble();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
            default:
                return null;
        }
    }
}
