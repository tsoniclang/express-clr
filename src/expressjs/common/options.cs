using System;
using System.Collections.Generic;

namespace expressjs;

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
    public int parameterLimit { get; set; } = 1000;
    public object? type { get; set; } = "application/x-www-form-urlencoded";
    public VerifyBodyHandler? verify { get; set; }
    public int depth { get; set; } = 32;
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
    public DateTime? expires { get; set; }
    public bool httpOnly { get; set; }
    public long? maxAge { get; set; }
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
    public long start { get; set; }
    public long end { get; set; }
}

public sealed class RangeResult
{
    public string type { get; set; } = "bytes";
    public List<ByteRange> ranges { get; } = new();
}

public sealed class FileStat
{
    public long size { get; set; }
    public DateTime modifiedAt { get; set; } = DateTime.UtcNow;
}
