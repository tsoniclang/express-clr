using System;
using System.Collections.Generic;
using Tsonic.JSRuntime;

namespace express;

public sealed class RouterOptions
{
    public bool caseSensitive { get; set; }
    public bool mergeParams { get; set; }
    public bool strict { get; set; }
}

public sealed class JsonOptions
{
    public bool inflate { get; set; } = true;
    public object? limit { get; set; } = "100kb";
    public object? reviver { get; set; }
    public bool strict { get; set; } = true;
    public object? type { get; set; } = "application/json";
    public VerifyBodyHandler? verify { get; set; }
}

public sealed class RawOptions
{
    public bool inflate { get; set; } = true;
    public object? limit { get; set; } = "100kb";
    public object? type { get; set; } = "application/octet-stream";
    public VerifyBodyHandler? verify { get; set; }
}

public sealed class TextOptions
{
    public string defaultCharset { get; set; } = "utf-8";
    public bool inflate { get; set; } = true;
    public object? limit { get; set; } = "100kb";
    public object? type { get; set; } = "text/plain";
    public VerifyBodyHandler? verify { get; set; }
}

public sealed class UrlEncodedOptions
{
    public bool extended { get; set; }
    public bool inflate { get; set; } = true;
    public object? limit { get; set; } = "100kb";
    private int _parameterLimit = 1000;
    public double parameterLimit
    {
        get => js_interop.fromInt32(_parameterLimit);
        set => _parameterLimit = js_interop.toInt32(nameof(parameterLimit), value);
    }
    public object? type { get; set; } = "application/x-www-form-urlencoded";
    public VerifyBodyHandler? verify { get; set; }
    private int _depth = 32;
    public double depth
    {
        get => js_interop.fromInt32(_depth);
        set => _depth = js_interop.toInt32(nameof(depth), value);
    }
}

public sealed class MultipartField
{
    public string name { get; set; } = string.Empty;
    private int? _maxCount;
    public double? maxCount
    {
        get => js_interop.fromInt32(_maxCount);
        set => _maxCount = js_interop.toNullableInt32(nameof(maxCount), value);
    }
}

public sealed class MultipartOptions
{
    public string type { get; set; } = "multipart/form-data";
    private int? _maxFileCount;
    private long? _maxFileSizeBytes;
    public double? maxFileCount
    {
        get => js_interop.fromInt32(_maxFileCount);
        set => _maxFileCount = js_interop.toNullableInt32(nameof(maxFileCount), value);
    }
    public double? maxFileSizeBytes
    {
        get => js_interop.fromInt64(_maxFileSizeBytes);
        set => _maxFileSizeBytes = js_interop.toNullableInt64(nameof(maxFileSizeBytes), value);
    }
}

public sealed class CorsOptions
{
    public string[]? origins { get; set; }
    public bool credentials { get; set; }
    public string[]? methods { get; set; }
    public string[]? allowedHeaders { get; set; }
    public string[]? exposedHeaders { get; set; }
    private int? _maxAgeSeconds;
    public double? maxAgeSeconds
    {
        get => js_interop.fromInt32(_maxAgeSeconds);
        set => _maxAgeSeconds = js_interop.toNullableInt32(nameof(maxAgeSeconds), value);
    }
    public bool preflightContinue { get; set; }
    private int _optionsSuccessStatus = 204;
    public double optionsSuccessStatus
    {
        get => js_interop.fromInt32(_optionsSuccessStatus);
        set => _optionsSuccessStatus = js_interop.toInt32(nameof(optionsSuccessStatus), value);
    }
}

public sealed class StaticOptions
{
    public string dotfiles { get; set; } = "ignore";
    public bool etag { get; set; } = true;
    public object? extensions { get; set; }
    public bool fallthrough { get; set; } = true;
    public bool immutable { get; set; }
    public object? index { get; set; } = "index.html";
    public bool lastModified { get; set; } = true;
    public object maxAge { get; set; } = 0;
    public bool redirect { get; set; } = true;
    public SetHeadersHandler? setHeaders { get; set; }
    public bool acceptRanges { get; set; } = true;
    public bool cacheControl { get; set; } = true;
}

public sealed class SendFileOptions
{
    public object maxAge { get; set; } = 0;
    public string? root { get; set; }
    public bool lastModified { get; set; } = true;
    public Dictionary<string, string> headers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public string dotfiles { get; set; } = "ignore";
    public bool acceptRanges { get; set; } = true;
    public bool cacheControl { get; set; } = true;
    public bool immutable { get; set; }
}

public sealed class DownloadOptions
{
    public object maxAge { get; set; } = 0;
    public string? root { get; set; }
    public bool lastModified { get; set; } = true;
    public Dictionary<string, string> headers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public string dotfiles { get; set; } = "ignore";
    public bool acceptRanges { get; set; } = true;
    public bool cacheControl { get; set; } = true;
    public bool immutable { get; set; }
}

public sealed class CookieOptions
{
    public string? domain { get; set; }
    public CookieEncoder? encode { get; set; }
    public Date? expires { get; set; }
    public bool httpOnly { get; set; }
    public double? maxAge { get; set; }
    public string path { get; set; } = "/";
    public bool partitioned { get; set; }
    public string? priority { get; set; }
    public bool secure { get; set; }
    public bool signed { get; set; }
    public object? sameSite { get; set; }
}

public sealed class RangeOptions
{
    public bool combine { get; set; }
}

public sealed class ByteRange
{
    public double start { get; set; }
    public double end { get; set; }
}

public sealed class RangeResult
{
    public string type { get; set; } = "bytes";
    public ByteRange[] ranges { get; set; } = Array.Empty<ByteRange>();

    internal void add(ByteRange range)
    {
        ranges = [.. ranges, range];
    }
}

public sealed class FileStat
{
    public double size { get; set; }
    public Date modifiedAt { get; set; } = new();
}
