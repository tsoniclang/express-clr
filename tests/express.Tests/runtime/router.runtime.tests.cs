using System.IO;
using System.Text;
using System.Threading.Tasks;
using express;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace express.Tests.runtime;

    public class router_runtime_tests
    {
        [Fact]
        public async Task middleware_and_route_handlers_run_in_order()
        {
        var app = express.create();

        app.use((RequestHandler)(async (_, res, next) =>
        {
            res.set("x-mw", "on");
            await next(null);
        }));

        app.get("/ping", static (Request _req, Response res, NextFunction _next) =>
        {
            res.status(200).send("pong");
            return Task.CompletedTask;
        });

        var context = createContext("GET", "/ping");
        await app.handle(context, app);

        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal("pong", readBody(context));
            Assert.Equal("on", context.Response.Headers["x-mw"].ToString());
        }

        [Fact]
        public async Task get_slash_matches_only_root_path()
        {
            var app = express.create();

            app.get("/", static (Request _req, Response res, NextFunction _next) =>
            {
                res.send("root");
                return Task.CompletedTask;
            });

            app.get("/items/:id", static (Request req, Response res, NextFunction _next) =>
            {
                res.send(req.@params["id"]?.ToString() ?? "");
                return Task.CompletedTask;
            });

            var rootContext = createContext("GET", "/");
            await app.handle(rootContext, app);
            Assert.Equal("root", readBody(rootContext));

            var itemContext = createContext("GET", "/items/123");
            await app.handle(itemContext, app);
            Assert.Equal("123", readBody(itemContext));
        }

        [Fact]
        public async Task next_route_skips_remaining_handlers_for_current_route()
        {
            var app = express.create();

        app.get("/item",
            (RequestHandler)(async (_, _, next) => await next("route")),
            (RequestHandler)(static (_, res, _) =>
            {
                res.send("wrong");
                return Task.CompletedTask;
            }));

        app.get("/item", static (Request _req, Response res, NextFunction _next) =>
        {
            res.send("ok");
            return Task.CompletedTask;
        });

        var context = createContext("GET", "/item");
        await app.handle(context, app);

        Assert.Equal("ok", readBody(context));
    }

    [Fact]
    public async Task app_param_runs_once_per_param_value_in_request_cycle()
    {
        var app = express.create();
        var invocations = 0;

        app.param("id", async (_, _, next, _, _) =>
        {
            invocations++;
            await next(null);
        });

        app.get("/users/:id", (RequestHandler)(static (_, _, next) => next(null)));
        app.get("/users/:id", static (Request _req, Response res, NextFunction _next) =>
        {
            res.send("done");
            return Task.CompletedTask;
        });

        var context = createContext("GET", "/users/42");
        await app.handle(context, app);

        Assert.Equal("done", readBody(context));
        Assert.Equal(1, invocations);
    }

    [Fact]
    public async Task app_use_with_subapp_triggers_mount_and_routes_requests()
    {
        var app = express.create();
        var child = express.create();
        var mounted = false;

        child.mount += _ => mounted = true;
        child.get("/hello", static (Request _req, Response res, NextFunction _next) =>
        {
            res.send("world");
            return Task.CompletedTask;
        });

        app.use("/api", child);

        var context = createContext("GET", "/api/hello");
        await app.handle(context, app);

        Assert.True(mounted);
        Assert.Equal("world", readBody(context));
    }

    [Fact]
    public async Task app_route_allows_chaining_http_handlers()
    {
        var app = express.create();

        app.route("/events")
            .get(static (Request _req, Response res, NextFunction _next) =>
            {
                res.send("get");
                return Task.CompletedTask;
            })
            .post(static (Request _req, Response res, NextFunction _next) =>
            {
                res.send("post");
                return Task.CompletedTask;
            });

        var getContext = createContext("GET", "/events");
        await app.handle(getContext, app);
        Assert.Equal("get", readBody(getContext));

        var postContext = createContext("POST", "/events");
        await app.handle(postContext, app);
        Assert.Equal("post", readBody(postContext));
    }

    [Fact]
    public async Task error_handler_with_four_args_is_invoked_after_thrown_error()
    {
        var app = express.create();

        app.get("/boom", (RequestHandler)(static (_, _, _) => throw new InvalidDataException("boom")));
        app.use((Func<System.Exception, Request, Response, NextFunction, Task>)(static (_, __, res, ___) =>
        {
            res.status(500).send("handled");
            return Task.CompletedTask;
        }));

        var context = createContext("GET", "/boom");
        await app.handle(context, app);

        Assert.Equal(500, context.Response.StatusCode);
        Assert.Equal("handled", readBody(context));
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
