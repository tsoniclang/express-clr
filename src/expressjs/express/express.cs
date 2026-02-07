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

#pragma warning disable IL2026
#pragma warning disable IL3050
            req.body = JsonSerializer.Deserialize<object>(body);
#pragma warning restore IL3050
#pragma warning restore IL2026
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
}
