using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using expressjs;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace expressjs.Tests.runtime;

public class middleware_runtime_tests
{
    [Fact]
    public async Task express_json_parses_json_payload()
    {
        var app = express.create();
        app.use(express.json());
        app.post("/json", (Action<Request, Response>)(static (req, res) => res.json(req.body)));

        var context = createContext("POST", "/json", "{\"a\":1}", "application/json");
        await app.handle(context, app);

        Assert.Equal("application/json", context.Response.ContentType);
        Assert.Contains("\"a\":1", readBody(context));
    }

    [Fact]
    public async Task express_text_parses_text_payload()
    {
        var app = express.create();
        app.use(express.text());
        app.post("/text", (Action<Request, Response>)(static (req, res) => res.send(req.body)));

        var context = createContext("POST", "/text", "hello", "text/plain");
        await app.handle(context, app);

        Assert.Equal("hello", readBody(context));
    }

    [Fact]
    public async Task express_raw_parses_binary_payload()
    {
        var app = express.create();
        app.use(express.raw());
        app.post("/raw", (Action<Request, Response>)(static (req, res) =>
        {
            var bytes = Assert.IsType<byte[]>(req.body);
            res.send(bytes.Length.ToString());
        }));

        var context = createContext("POST", "/raw", "abcd", "application/octet-stream");
        await app.handle(context, app);

        Assert.Equal("4", readBody(context));
    }

    [Fact]
    public async Task express_urlencoded_parses_form_payload()
    {
        var app = express.create();
        app.use(express.urlencoded(new UrlEncodedOptions { extended = true }));
        app.post("/form", (Action<Request, Response>)(static (req, res) =>
        {
            var body = Assert.IsType<Dictionary<string, object?>>(req.body);
            res.send(body["name"]?.ToString());
        }));

        var context = createContext("POST", "/form", "name=tsonic", "application/x-www-form-urlencoded");
        await app.handle(context, app);

        Assert.Equal("tsonic", readBody(context));
    }

    [Fact]
    public async Task response_cookie_sets_set_cookie_header()
    {
        var app = express.create();
        app.get("/cookie", (Action<Request, Response>)(static (_, res) =>
        {
            res.cookie("session", "abc", new CookieOptions { path = "/" }).send("ok");
        }));

        var context = createContext("GET", "/cookie");
        await app.handle(context, app);

        Assert.Contains("session=abc", context.Response.Headers["Set-Cookie"].ToString());
    }

    [Fact]
    public async Task response_render_uses_registered_engine()
    {
        var app = express.create();
        app.engine("tpl", static (_, locals, callback) =>
        {
            callback(null, $"hello {locals["name"]}");
        });

        app.get("/view", (Action<Request, Response>)(static (_, res) =>
        {
            res.render("home.tpl", new Dictionary<string, object?> { ["name"] = "world" });
        }));

        var context = createContext("GET", "/view");
        await app.handle(context, app);

        Assert.Equal("hello world", readBody(context));
    }

    [Fact]
    public async Task response_jsonp_uses_configured_callback_name()
    {
        var app = express.create();
        app.set("jsonp callback name", "cb");
        app.get("/jsonp", (Action<Request, Response>)(static (_, res) => res.jsonp(new { ok = true })));

        var context = createContext("GET", "/jsonp");
        await app.handle(context, app);

        Assert.StartsWith("cb(", readBody(context));
    }

    [Fact]
    public async Task express_static_serves_files_from_root()
    {
        var app = express.create();
        var root = Path.Combine(Path.GetTempPath(), $"expressjs-static-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, "asset.txt"), "asset");

        try
        {
            app.use(express.@static(root));
            var context = createContext("GET", "/asset.txt");
            await app.handle(context, app);
            Assert.Equal("asset", readBody(context));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    private static DefaultHttpContext createContext(string method, string path, string? body = null, string? contentType = null)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        context.Response.Body = new MemoryStream();

        if (body is not null)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            context.Request.Body = new MemoryStream(bytes);
            context.Request.ContentType = contentType;
        }

        return context;
    }

    private static string readBody(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true);
        return reader.ReadToEnd();
    }
}
