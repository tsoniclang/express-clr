using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using express;
using Xunit;

namespace express.Tests.runtime;

public class coverage_matrix_application_tests
{
    [Fact]
    public void app_router_property_and_setting_helpers_round_trip_values()
    {
        var app = express.create();
        Assert.Same(app, app.router);
        Assert.False(app.disabled("trust proxy"));
        Assert.False(app.enabled("trust proxy"));

        app.disable("trust proxy");
        Assert.True(app.disabled("trust proxy"));
        Assert.False(app.enabled("trust proxy"));

        app.enable("trust proxy");
        Assert.True(app.enabled("trust proxy"));
        Assert.False(app.disabled("trust proxy"));

        app.set("trust proxy", "not-bool");
        Assert.False(app.enabled("trust proxy"));

        app.set("title", "matrix");
        Assert.Equal("matrix", app.get("title"));
    }

    [Fact]
    public void app_path_covers_string_collection_and_fallback_variants()
    {
        var app = express.create();

        app.mountpath = "/admin";
        Assert.Equal("/admin", app.path());

        app.mountpath = new[] { "/a", "/b" };
        Assert.Equal("/a,/b", app.path());

        app.mountpath = 42;
        Assert.Equal(string.Empty, app.path());
    }

    [Fact]
    public async Task app_param_array_and_engine_render_overloads_work()
    {
        var app = express.create();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        app.param(new[] { "id", "page" }, async (_, _, next, _, name) =>
        {
            seen.Add(name);
            await next(null);
        });

        app.get("/users/:id/:page", (Action<Request, Response>)(static (_, res) => res.send("ok")));

        var context = test_runtime_utils.createContext("GET", "/users/42/3");
        await test_runtime_utils.run(app, context);
        Assert.Equal("ok", test_runtime_utils.readBody(context));
        Assert.Contains("id", seen);
        Assert.Contains("page", seen);

        app.engine(".tpl", static (_, locals, callback) => callback(null, $"name={locals["name"]}"));

        string? renderedWithoutExtraLocals = null;
        app.locals["name"] = "alex";
        app.render("welcome.tpl", (_, html) => renderedWithoutExtraLocals = html);
        Assert.Equal("name=alex", renderedWithoutExtraLocals);

        string? renderedWithLocals = null;
        app.render("welcome.tpl", new Dictionary<string, object?> { ["name"] = "sam" }, (_, html) => renderedWithLocals = html);
        Assert.Equal("name=sam", renderedWithLocals);

        string? fallback = null;
        app.render("missing.view", (_, html) => fallback = html);
        Assert.Equal("<rendered:missing.view>", fallback);

        string? fallbackWithLocals = null;
        app.render("missing.view", new Dictionary<string, object?> { ["name"] = "extra" }, (_, html) => fallbackWithLocals = html);
        Assert.Equal("<rendered:missing.view>", fallbackWithLocals);

        string? trailingDot = null;
        app.render("index.", (_, html) => trailingDot = html);
        Assert.Equal("<rendered:index.>", trailingDot);
    }

    [Fact]
    public async Task app_use_mounts_child_apps_and_emits_mount_event_for_all_callbacks()
    {
        var app = express.create();
        var childA = express.create();
        var childB = express.create();
        var mountedA = false;
        var mountedB = false;

        childA.mount += parent =>
        {
            mountedA = ReferenceEquals(parent, app);
        };
        childB.mount += parent =>
        {
            mountedB = ReferenceEquals(parent, app);
        };

        childA.get("/a", (Action<Request, Response>)(static (_, res) => res.send("A")));
        childB.get("/b", (Action<Request, Response>)(static (_, res) => res.send("B")));

        app.use("/api", childA, childB);

        Assert.True(mountedA);
        Assert.True(mountedB);
        Assert.Equal("/api", childA.mountpath);
        Assert.Equal("/api", childB.mountpath);

        var contextA = test_runtime_utils.createContext("GET", "/api/a");
        await test_runtime_utils.run(app, contextA);
        Assert.Equal("A", test_runtime_utils.readBody(contextA));

        var contextB = test_runtime_utils.createContext("GET", "/api/b");
        await test_runtime_utils.run(app, contextB);
        Assert.Equal("B", test_runtime_utils.readBody(contextB));

        var childC = express.create();
        var childD = express.create();
        app.use(callback: childC, callbacks: new object[] { childD });
        Assert.Equal("/", childC.mountpath);
        Assert.Equal("/", childD.mountpath);
    }

    [Fact]
    public async Task app_listen_overloads_return_active_servers_that_can_be_closed()
    {
        var app1 = express.create();
        var callback1 = false;
        var port1 = test_runtime_utils.reserveTcpPort();
        var server1 = app1.listen(port1, () => callback1 = true);
        Assert.True(callback1);
        Assert.True(server1.listening);
        Assert.NotNull(server1.keepAliveThread);
        Assert.False(server1.keepAliveThread!.IsBackground);
        Assert.True(server1.keepAliveThread.IsAlive);
        Assert.Equal(port1, server1.port);
        Assert.Null(server1.host);
        Assert.Null(server1.path);

        using (var client = new System.Net.Http.HttpClient())
        {
            var response = await client.GetAsync($"http://127.0.0.1:{port1}/not-found");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        server1.close();
        Assert.False(server1.listening);
        Assert.False(server1.keepAliveThread!.IsAlive);

        var app2 = express.create();
        var callback2 = false;
        var port2 = test_runtime_utils.reserveTcpPort();
        var server2 = app2.listen(port2, "127.0.0.1", () => callback2 = true);
        Assert.True(callback2);
        Assert.NotNull(server2.keepAliveThread);
        Assert.False(server2.keepAliveThread!.IsBackground);
        Assert.True(server2.keepAliveThread.IsAlive);
        Assert.Equal(port2, server2.port);
        Assert.Equal("127.0.0.1", server2.host);
        server2.close();
        Assert.False(server2.listening);
        Assert.False(server2.keepAliveThread!.IsAlive);

        var app3 = express.create();
        var callback3 = false;
        var port3 = test_runtime_utils.reserveTcpPort();
        var server3 = app3.listen(port3, "127.0.0.1", 128, () => callback3 = true);
        Assert.True(callback3);
        Assert.NotNull(server3.keepAliveThread);
        Assert.False(server3.keepAliveThread!.IsBackground);
        Assert.True(server3.keepAliveThread.IsAlive);
        Assert.Equal(port3, server3.port);
        Assert.Equal("127.0.0.1", server3.host);
        server3.close();
        Assert.False(server3.listening);
        Assert.False(server3.keepAliveThread!.IsAlive);

        var app4 = express.create();
        var callback4 = false;
        var socketPath = Path.Combine(Path.GetTempPath(), $"express-{Guid.NewGuid():N}.sock");
        var server4 = app4.listen(socketPath, () => callback4 = true);
        Assert.True(callback4);
        Assert.NotNull(server4.keepAliveThread);
        Assert.False(server4.keepAliveThread!.IsBackground);
        Assert.True(server4.keepAliveThread.IsAlive);
        Assert.Equal(socketPath, server4.path);

        var closed = false;
        server4.close(_ => closed = true);
        Assert.True(closed);
        Assert.False(server4.listening);
        Assert.False(server4.keepAliveThread!.IsAlive);
    }
}
