using System;
using System.Linq;
using express;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace express.Tests.runtime;

public class coverage_matrix_request_tests
{
    [Fact]
    public void from_http_context_populates_core_request_properties()
    {
        var app = express.create();
        app.set("subdomain offset", 2);

        var context = test_runtime_utils.createContext("GET", "/users/42", queryString: "?a=1&a=2&b=3", host: "a.b.example.com");
        context.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
        context.Request.Headers["Accept-Language"] = "en-US,en;q=0.9";
        context.Request.Headers["Cookie"] = "sid=abc";

        var req = Request.fromHttpContext(context, app);

        Assert.Equal("GET", req.method);
        Assert.Equal("/users/42", req.path);
        Assert.Equal("/users/42?a=1&a=2&b=3", req.originalUrl);
        Assert.Equal("http", req.protocol);
        Assert.Equal("a.b.example.com", req.host);
        Assert.Equal("a.b.example.com", req.hostname);
        Assert.Equal("abc", req.cookies["sid"]);
        Assert.True(req.xhr);
        Assert.Equal("3", req.query["b"]);
        var queryArray = Assert.IsType<string[]>(req.query["a"]);
        Assert.Equal(new[] { "1", "2" }, queryArray);
        Assert.Equal(new[] { "b", "a" }, req.subdomains);
    }

    [Fact]
    public void accepts_helpers_cover_empty_and_non_empty_paths()
    {
        var req = new Request();

        Assert.Equal(false, req.accepts());
        Assert.Equal(false, req.acceptsCharsets());
        Assert.Equal(false, req.acceptsEncodings());

        Assert.Equal("json", req.accepts("json", "html"));
        Assert.Equal("utf-8", req.acceptsCharsets("utf-8", "ascii"));
        Assert.Equal("gzip", req.acceptsEncodings("gzip", "br"));
        Assert.Equal("en", req.acceptsLanguages("en", "fr"));
    }

    [Fact]
    public void accepts_languages_without_arguments_parses_header_or_returns_empty()
    {
        var withHeader = new Request();
        withHeader.setHeader("Accept-Language", "en-US,en;q=0.9,fr;q=0.8");

        var parsed = Assert.IsType<string[]>(withHeader.acceptsLanguages());
        Assert.Equal(new[] { "en-US", "en", "fr" }, parsed);

        var withoutHeader = new Request();
        var empty = Assert.IsType<string[]>(withoutHeader.acceptsLanguages());
        Assert.Empty(empty);
    }

    [Fact]
    public void get_and_header_read_from_internal_headers_and_context()
    {
        var req = new Request();
        req.setHeader("x-one", "1");
        Assert.Equal("1", req.get("x-one"));
        Assert.Equal("1", req.header("x-one"));
        Assert.Null(req.get("missing"));

        var context = test_runtime_utils.createContext("GET", "/");
        context.Request.Headers["x-two"] = "2";
        var fromContext = Request.fromHttpContext(context, express.create());
        Assert.Equal("2", fromContext.get("x-two"));
        Assert.Null(fromContext.get("x-missing"));

        context.Request.Headers["x-late"] = "late";
        Assert.Equal("late", fromContext.get("x-late"));
    }

    [Fact]
    public void is_and_range_cover_all_branches()
    {
        var req = new Request();
        Assert.Null(req.@is());
        Assert.Null(req.@is("json"));

        req.body = "data";
        Assert.Equal("json", req.@is("json", "text"));

        Assert.Equal(-1, req.range(0));
        var range = Assert.IsType<RangeResult>(req.range(10));
        Assert.Equal("bytes", range.type);
        Assert.Single(range.ranges);
        Assert.Equal(0, range.ranges[0].start);
        Assert.Equal(9, range.ranges[0].end);
    }

    [Fact]
    public void secure_and_stale_flags_reflect_protocol_and_freshness()
    {
        var req = new Request { protocol = "https", fresh = true };
        Assert.True(req.secure);
        Assert.False(req.stale);

        req.protocol = "http";
        req.fresh = false;
        Assert.False(req.secure);
        Assert.True(req.stale);
    }

    [Fact]
    public void param_reads_from_params_dictionary_and_coerces_to_string()
    {
        var req = new Request();
        req.@params.Set("id", "42");
        Assert.Equal("42", req.param("id"));
        Assert.Null(req.param("missing"));

        req.@params.Set("n", 123);
        Assert.Equal("123", req.param("n"));
    }

    [Fact]
    public void from_http_context_handles_empty_path_missing_host_and_null_app()
    {
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Scheme = "http";
        context.Connection.RemoteIpAddress = null;

        var req = Request.fromHttpContext(context, null);
        Assert.Equal("/", req.path);
        Assert.Equal(string.Empty, req.host);
        Assert.Equal(string.Empty, req.ip);
        Assert.Empty(req.subdomains);

        var withNullHost = new DefaultHttpContext();
        withNullHost.Request.Method = "GET";
        withNullHost.Request.Scheme = "http";
        withNullHost.Request.Path = "/test";
        withNullHost.Request.Host = new HostString((string?)null!);
        var reqWithNullHost = Request.fromHttpContext(withNullHost, express.create());
        Assert.Equal(string.Empty, reqWithNullHost.host);

        var withHostAndNullApp = test_runtime_utils.createContext("GET", "/test", host: "a.b.example.com");
        var reqWithHostAndNullApp = Request.fromHttpContext(withHostAndNullApp, null);
        Assert.Equal(new[] { "b", "a" }, reqWithHostAndNullApp.subdomains);
    }
}
