using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using express;
using Xunit;

namespace express.Tests.runtime;

public class coverage_matrix_infrastructure_tests
{
    private sealed class dummy_host : RoutingHost<dummy_host>
    {
    }

    [Fact]
    public void routing_host_default_methods_return_self_and_default_route_throws()
    {
        var host = new dummy_host();
        var callback = (Action<Request, Response>)(static (_, _) => { });

        Assert.Same(host, host.all("/", callback));
        Assert.Same(host, host.checkout("/", callback));
        Assert.Same(host, host.copy("/", callback));
        Assert.Same(host, host.delete("/", callback));
        Assert.Same(host, host.get("/", callback));
        Assert.Same(host, host.head("/", callback));
        Assert.Same(host, host.lock_("/", callback));
        Assert.Same(host, host.merge("/", callback));
        Assert.Same(host, host.method("GET", "/", callback));
        Assert.Same(host, host.mkactivity("/", callback));
        Assert.Same(host, host.mkcol("/", callback));
        Assert.Same(host, host.move("/", callback));
        Assert.Same(host, host.m_search("/", callback));
        Assert.Same(host, host.notify("/", callback));
        Assert.Same(host, host.options("/", callback));
        Assert.Same(host, host.patch("/", callback));
        Assert.Same(host, host.post("/", callback));
        Assert.Same(host, host.purge("/", callback));
        Assert.Same(host, host.put("/", callback));
        Assert.Same(host, host.report("/", callback));
        Assert.Same(host, host.search("/", callback));
        Assert.Same(host, host.subscribe("/", callback));
        Assert.Same(host, host.trace("/", callback));
        Assert.Same(host, host.unlock("/", callback));
        Assert.Same(host, host.unsubscribe("/", callback));
        Assert.Same(host, host.param("id", static (_, _, next, _, _) => next(null)));
        Assert.Same(host, host.use(callback));
        Assert.Same(host, host.use("/", callback));

        Assert.Throws<NotSupportedException>(() => host.route("/"));
    }

    [Fact]
    public void options_and_support_types_cover_all_getters()
    {
        var routerOptions = new RouterOptions { caseSensitive = true, mergeParams = true, strict = true };
        Assert.True(routerOptions.caseSensitive);
        Assert.True(routerOptions.mergeParams);
        Assert.True(routerOptions.strict);

        var jsonOptions = new JsonOptions
        {
            inflate = false,
            limit = 10,
            reviver = (Func<object?, object?>)(value => value),
            strict = false,
            type = "application/custom+json",
            verify = static (_, _, _, _) => { }
        };
        Assert.False(jsonOptions.inflate);
        Assert.Equal(10, jsonOptions.limit);
        Assert.NotNull(jsonOptions.reviver);
        Assert.False(jsonOptions.strict);
        Assert.Equal("application/custom+json", jsonOptions.type);
        Assert.NotNull(jsonOptions.verify);

        var rawOptions = new RawOptions { inflate = false, limit = 11, type = "application/custom-raw", verify = static (_, _, _, _) => { } };
        Assert.False(rawOptions.inflate);
        Assert.Equal(11, rawOptions.limit);
        Assert.Equal("application/custom-raw", rawOptions.type);
        Assert.NotNull(rawOptions.verify);

        var textOptions = new TextOptions
        {
            defaultCharset = "utf-16",
            inflate = false,
            limit = 12,
            type = "text/custom",
            verify = static (_, _, _, _) => { }
        };
        Assert.Equal("utf-16", textOptions.defaultCharset);
        Assert.False(textOptions.inflate);
        Assert.Equal(12, textOptions.limit);
        Assert.Equal("text/custom", textOptions.type);
        Assert.NotNull(textOptions.verify);

        var urlencodedOptions = new UrlEncodedOptions
        {
            extended = true,
            inflate = false,
            limit = 13,
            parameterLimit = 20,
            type = "application/x-custom-form",
            verify = static (_, _, _, _) => { },
            depth = 8
        };
        Assert.True(urlencodedOptions.extended);
        Assert.False(urlencodedOptions.inflate);
        Assert.Equal(13, urlencodedOptions.limit);
        Assert.Equal(20, urlencodedOptions.parameterLimit);
        Assert.Equal("application/x-custom-form", urlencodedOptions.type);
        Assert.NotNull(urlencodedOptions.verify);
        Assert.Equal(8, urlencodedOptions.depth);

        var staticOptions = new StaticOptions
        {
            dotfiles = "allow",
            etag = false,
            extensions = new[] { "html" },
            fallthrough = false,
            immutable = true,
            index = false,
            lastModified = false,
            maxAge = "1d",
            redirect = false,
            setHeaders = static (_, _, _) => { },
            acceptRanges = false,
            cacheControl = false
        };
        Assert.Equal("allow", staticOptions.dotfiles);
        Assert.False(staticOptions.etag);
        Assert.NotNull(staticOptions.extensions);
        Assert.False(staticOptions.fallthrough);
        Assert.True(staticOptions.immutable);
        Assert.False((bool)staticOptions.index);
        Assert.False(staticOptions.lastModified);
        Assert.Equal("1d", staticOptions.maxAge);
        Assert.False(staticOptions.redirect);
        Assert.NotNull(staticOptions.setHeaders);
        Assert.False(staticOptions.acceptRanges);
        Assert.False(staticOptions.cacheControl);

        var sendFileOptions = new SendFileOptions
        {
            maxAge = 1,
            root = "/tmp",
            lastModified = false,
            headers = new Dictionary<string, string> { ["x"] = "1" },
            dotfiles = "deny",
            acceptRanges = false,
            cacheControl = false,
            immutable = true
        };
        Assert.Equal(1, sendFileOptions.maxAge);
        Assert.Equal("/tmp", sendFileOptions.root);
        Assert.False(sendFileOptions.lastModified);
        Assert.Equal("1", sendFileOptions.headers["x"]);
        Assert.Equal("deny", sendFileOptions.dotfiles);
        Assert.False(sendFileOptions.acceptRanges);
        Assert.False(sendFileOptions.cacheControl);
        Assert.True(sendFileOptions.immutable);

        var downloadOptions = new DownloadOptions
        {
            maxAge = 2,
            root = "/tmp/root",
            lastModified = false,
            headers = new Dictionary<string, string> { ["y"] = "2" },
            dotfiles = "allow",
            acceptRanges = false,
            cacheControl = false,
            immutable = true
        };
        Assert.Equal(2, downloadOptions.maxAge);
        Assert.Equal("/tmp/root", downloadOptions.root);
        Assert.False(downloadOptions.lastModified);
        Assert.Equal("2", downloadOptions.headers["y"]);
        Assert.Equal("allow", downloadOptions.dotfiles);
        Assert.False(downloadOptions.acceptRanges);
        Assert.False(downloadOptions.cacheControl);
        Assert.True(downloadOptions.immutable);

        var cookieOptions = new CookieOptions
        {
            domain = "example.com",
            encode = static value => value,
            expires = DateTime.UtcNow,
            httpOnly = true,
            maxAge = 5000,
            path = "/admin",
            partitioned = true,
            priority = "high",
            secure = true,
            signed = true,
            sameSite = "Strict"
        };
        Assert.Equal("example.com", cookieOptions.domain);
        Assert.NotNull(cookieOptions.encode);
        Assert.NotNull(cookieOptions.expires);
        Assert.True(cookieOptions.httpOnly);
        Assert.Equal(5000, cookieOptions.maxAge);
        Assert.Equal("/admin", cookieOptions.path);
        Assert.True(cookieOptions.partitioned);
        Assert.Equal("high", cookieOptions.priority);
        Assert.True(cookieOptions.secure);
        Assert.True(cookieOptions.signed);
        Assert.Equal("Strict", cookieOptions.sameSite);

        var rangeOptions = new RangeOptions { combine = true };
        Assert.True(rangeOptions.combine);

        var byteRange = new ByteRange { start = 1, end = 2 };
        Assert.Equal(1, byteRange.start);
        Assert.Equal(2, byteRange.end);

        var rangeResult = new RangeResult();
        rangeResult.type = "bytes";
        rangeResult.ranges.Add(byteRange);
        Assert.Equal("bytes", rangeResult.type);
        Assert.Single(rangeResult.ranges);

        var stat = new FileStat { size = 9, modifiedAt = DateTime.UtcNow };
        Assert.Equal(9, stat.size);
        Assert.True(stat.modifiedAt > DateTime.UnixEpoch);

        var request = new Request { signed = true };
        Assert.True(request.signed);
    }

    [Fact]
    public void app_server_close_reports_callback_errors()
    {
        var cleanServer = new AppServer(1, "127.0.0.1", null);
        cleanServer.close();
        Assert.False(cleanServer.listening);

        var server = new AppServer(1, "127.0.0.1", null, () => throw new InvalidOperationException("close"));
        Exception? callbackError = null;
        server.close(err => callbackError = err);

        Assert.NotNull(callbackError);
        Assert.IsType<InvalidOperationException>(callbackError);
    }
}
