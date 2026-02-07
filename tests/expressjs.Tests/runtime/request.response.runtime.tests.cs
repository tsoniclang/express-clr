using System.Collections.Generic;
using expressjs;
using Xunit;

namespace expressjs.Tests.runtime;

public class request_response_runtime_tests
{
    [Fact]
    public void request_header_methods_and_security_flags_work()
    {
        var req = new Request
        {
            protocol = "https",
            fresh = true,
            body = "hello"
        };

        req.setHeader("Content-Type", "application/json");

        Assert.Equal("application/json", req.get("Content-Type"));
        Assert.Equal("application/json", req.header("content-type"));
        Assert.True(req.secure);
        Assert.True(req.fresh);
        Assert.False(req.stale);
        Assert.Equal("json", req.@is("json", "text"));
    }

    [Fact]
    public void request_accept_helpers_and_range_return_expected_shapes()
    {
        var req = new Request();

        Assert.Equal("json", req.accepts("json", "html"));
        Assert.Equal("utf-8", req.acceptsCharsets("utf-8", "ascii"));
        Assert.Equal("gzip", req.acceptsEncodings("gzip", "br"));
        Assert.Equal("en", req.acceptsLanguages("en", "fr"));

        var result = Assert.IsType<RangeResult>(req.range(10));
        Assert.Equal("bytes", result.type);
        Assert.Single(result.ranges);
        Assert.Equal(0, result.ranges[0].start);
        Assert.Equal(9, result.ranges[0].end);
    }

    [Fact]
    public void response_header_and_cookie_helpers_work_without_http_context()
    {
        var res = new Response();

        res.append("Warning", "199 misc");
        res.append("Warning", "299 extra");
        Assert.Contains("199 misc", res.get("Warning"));
        Assert.Contains("299 extra", res.get("Warning"));

        res.cookie("token", "abc");
        Assert.Contains("token=abc", res.get("Set-Cookie"));

        res.clearCookie("token", new CookieOptions { path = "/" });
        Assert.Contains("token=", res.get("Set-Cookie"));
    }

    [Fact]
    public void response_status_type_sendstatus_and_vary_work()
    {
        var res = new Response();

        res.status(418).type("json").vary("Accept").sendStatus(404);

        Assert.Equal(404, res.statusCode);
        Assert.True(res.headersSent);
        Assert.Equal("application/json", res.get("Content-Type"));
        Assert.Equal("Accept", res.get("Vary"));
    }

    [Fact]
    public void response_links_location_redirect_set_and_header_aliases_work()
    {
        var res = new Response();

        res.links(new Dictionary<string, string>
        {
            ["next"] = "http://api.example.com/page/2",
            ["last"] = "http://api.example.com/page/9"
        });
        Assert.Contains("rel=\"next\"", res.get("Link"));
        Assert.Contains("rel=\"last\"", res.get("Link"));

        res.location("/admin");
        Assert.Equal("/admin", res.get("Location"));

        res.set("x-one", "1");
        res.header("x-two", "2");
        Assert.Equal("1", res.get("x-one"));
        Assert.Equal("2", res.get("x-two"));

        res.redirect(301, "/moved");
        Assert.Equal(301, res.statusCode);
        Assert.Equal("/moved", res.get("Location"));
    }

    [Fact]
    public void response_render_and_format_execute_handlers()
    {
        var res = new Response();

        res.render("index");
        Assert.True(res.headersSent);

        var used = "";
        res.format(new Dictionary<string, System.Action>
        {
            ["application/json"] = () => used = "json",
            ["default"] = () => used = "default"
        });

        Assert.Equal("default", used);
    }

    [Fact]
    public void response_json_and_jsonp_emit_expected_payload_forms()
    {
        var app = express.create();
        app.set("jsonp callback name", "cb");

        var req = new Request { app = app };
        var res = new Response { req = req };

        res.json(new { ok = true });
        Assert.Equal("application/json", res.get("Content-Type"));

        res.jsonp(new { ok = true });
        Assert.Equal("application/javascript", res.get("Content-Type"));
    }
}
