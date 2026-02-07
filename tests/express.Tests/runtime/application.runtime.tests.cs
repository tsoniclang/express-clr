using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using express;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace express.Tests.runtime;

public class application_runtime_tests
{
    [Fact]
    public void enable_disable_and_set_get_roundtrip_settings()
    {
        var app = express.create();

        app.disable("trust proxy");
        Assert.True(app.disabled("trust proxy"));
        Assert.False(app.enabled("trust proxy"));

        app.enable("trust proxy");
        Assert.True(app.enabled("trust proxy"));
        Assert.False(app.disabled("trust proxy"));

        app.set("title", "my-site");
        Assert.Equal("my-site", app.get("title"));
    }

    [Fact]
    public void app_path_reflects_mountpath()
    {
        var app = express.create();
        app.mountpath = "/admin";
        Assert.Equal("/admin", app.path());

        app.mountpath = new[] { "/a", "/b" };
        Assert.Equal("/a,/b", app.path());
    }

    [Fact]
    public async Task app_param_array_registers_multiple_handlers()
    {
        var app = express.create();
        var seen = new HashSet<string>();

        app.param(new[] { "id", "page" }, async (_, _, next, _, name) =>
        {
            seen.Add(name);
            await next(null);
        });

        app.get("/users/:id/:page", (Action<Request, Response>)(static (_, res) => res.send("ok")));

        var context = createContext("GET", "/users/42/3");
        await app.handle(context, app);

        Assert.Equal("ok", readBody(context));
        Assert.Contains("id", seen);
        Assert.Contains("page", seen);
    }

    [Fact]
    public async Task app_render_uses_registered_engine_callback()
    {
        var app = express.create();
        app.engine("tpl", static (_, locals, callback) => callback(null, $"name={locals["name"]}"));

        string? rendered = null;
        app.render("welcome.tpl", new Dictionary<string, object?> { ["name"] = "alex" }, (_, html) => rendered = html);

        await Task.CompletedTask;
        Assert.Equal("name=alex", rendered);
    }

    [Fact]
    public async Task app_all_matches_all_http_methods()
    {
        var app = express.create();
        app.all("/health", (Action<Request, Response>)(static (_, res) => res.send("ok")));

        var getContext = createContext("GET", "/health");
        await app.handle(getContext, app);
        Assert.Equal("ok", readBody(getContext));

        var postContext = createContext("POST", "/health");
        await app.handle(postContext, app);
        Assert.Equal("ok", readBody(postContext));
    }

    private static DefaultHttpContext createContext(string method, string path)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static string readBody(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true);
        return reader.ReadToEnd();
    }
}
