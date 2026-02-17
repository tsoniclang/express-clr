using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using express;
using Xunit;

namespace express.Tests.runtime;

public class coverage_matrix_routing_tests
{
    private delegate void register_app_verb(Application app, string path, Action<Request, Response> handler);
    private delegate void register_router_verb(Router router, string path, Action<Request, Response> handler);
    private delegate void register_route_verb(Route route, Action<Request, Response> handler);

    private sealed record verb_case(
        string name,
        string http_method,
        register_app_verb app_register,
        register_router_verb router_register,
        register_route_verb route_register);

    private static readonly verb_case[] cases =
    {
        new("all", "GET",
            static (app, path, handler) => app.all(path, handler),
            static (router, path, handler) => router.all(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).all("/ignored", handler)),
        new("checkout", "CHECKOUT",
            static (app, path, handler) => app.checkout(path, handler),
            static (router, path, handler) => router.checkout(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).checkout("/ignored", handler)),
        new("copy", "COPY",
            static (app, path, handler) => app.copy(path, handler),
            static (router, path, handler) => router.copy(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).copy("/ignored", handler)),
        new("delete", "DELETE",
            static (app, path, handler) => app.delete(path, handler),
            static (router, path, handler) => router.delete(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).delete("/ignored", handler)),
        new("get", "GET",
            static (app, path, handler) => app.get(path, handler),
            static (router, path, handler) => router.get(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).get("/ignored", handler)),
        new("head", "HEAD",
            static (app, path, handler) => app.head(path, handler),
            static (router, path, handler) => router.head(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).head("/ignored", handler)),
        new("lock", "LOCK",
            static (app, path, handler) => app.lock_(path, handler),
            static (router, path, handler) => router.lock_(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).lock_("/ignored", handler)),
        new("merge", "MERGE",
            static (app, path, handler) => app.merge(path, handler),
            static (router, path, handler) => router.merge(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).merge("/ignored", handler)),
        new("mkactivity", "MKACTIVITY",
            static (app, path, handler) => app.mkactivity(path, handler),
            static (router, path, handler) => router.mkactivity(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).mkactivity("/ignored", handler)),
        new("mkcol", "MKCOL",
            static (app, path, handler) => app.mkcol(path, handler),
            static (router, path, handler) => router.mkcol(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).mkcol("/ignored", handler)),
        new("move", "MOVE",
            static (app, path, handler) => app.move(path, handler),
            static (router, path, handler) => router.move(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).move("/ignored", handler)),
        new("m-search", "M-SEARCH",
            static (app, path, handler) => app.m_search(path, handler),
            static (router, path, handler) => router.m_search(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).m_search("/ignored", handler)),
        new("notify", "NOTIFY",
            static (app, path, handler) => app.notify(path, handler),
            static (router, path, handler) => router.notify(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).notify("/ignored", handler)),
        new("options", "OPTIONS",
            static (app, path, handler) => app.options(path, handler),
            static (router, path, handler) => router.options(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).options("/ignored", handler)),
        new("patch", "PATCH",
            static (app, path, handler) => app.patch(path, handler),
            static (router, path, handler) => router.patch(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).patch("/ignored", handler)),
        new("post", "POST",
            static (app, path, handler) => app.post(path, handler),
            static (router, path, handler) => router.post(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).post("/ignored", handler)),
        new("purge", "PURGE",
            static (app, path, handler) => app.purge(path, handler),
            static (router, path, handler) => router.purge(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).purge("/ignored", handler)),
        new("put", "PUT",
            static (app, path, handler) => app.put(path, handler),
            static (router, path, handler) => router.put(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).put("/ignored", handler)),
        new("report", "REPORT",
            static (app, path, handler) => app.report(path, handler),
            static (router, path, handler) => router.report(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).report("/ignored", handler)),
        new("search", "SEARCH",
            static (app, path, handler) => app.search(path, handler),
            static (router, path, handler) => router.search(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).search("/ignored", handler)),
        new("subscribe", "SUBSCRIBE",
            static (app, path, handler) => app.subscribe(path, handler),
            static (router, path, handler) => router.subscribe(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).subscribe("/ignored", handler)),
        new("trace", "TRACE",
            static (app, path, handler) => app.trace(path, handler),
            static (router, path, handler) => router.trace(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).trace("/ignored", handler)),
        new("unlock", "UNLOCK",
            static (app, path, handler) => app.unlock(path, handler),
            static (router, path, handler) => router.unlock(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).unlock("/ignored", handler)),
        new("unsubscribe", "UNSUBSCRIBE",
            static (app, path, handler) => app.unsubscribe(path, handler),
            static (router, path, handler) => router.unsubscribe(path, handler),
            static (route, handler) => ((RoutingHost<Route>)route).unsubscribe("/ignored", handler))
    };

    [Fact]
    public async Task every_app_router_and_route_http_method_registers_and_handles_requests()
    {
        foreach (var item in cases)
        {
            var appPath = $"/app/{item.name}";
            var app = express.create();
            item.app_register(app, appPath, static (_, res) => res.send("ok-app"));
            await assertRoute(app, item.http_method, appPath, "ok-app");

            var routedPath = $"/r/{item.name}";
            var withRouter = express.create();
            var router = express.Router();
            item.router_register(router, $"/{item.name}", static (_, res) => res.send("ok-router"));
            withRouter.use("/r", router);
            await assertRoute(withRouter, item.http_method, routedPath, "ok-router");

            var routePath = $"/route/{item.name}";
            var withRoute = express.create();
            var route = withRoute.route(routePath);
            item.route_register(route, (Action<Request, Response>)(static (_, res) => res.send("ok-route")));
            await assertRoute(withRoute, item.http_method, routePath, "ok-route");
        }
    }

    [Fact]
    public async Task method_fallback_handles_verbs_with_or_without_dashes()
    {
        var app = express.create();
        app.method("M-SEARCH", "/m", (Action<Request, Response>)(static (_, res) => res.send("app")));
        await assertRoute(app, "M-SEARCH", "/m", "app");

        var viaRouter = express.create();
        var router = express.Router();
        router.method("LOCK", "/x", (Action<Request, Response>)(static (_, res) => res.send("router")));
        viaRouter.use("/api", router);
        await assertRoute(viaRouter, "LOCK", "/api/x", "router");

        var viaRoute = express.create();
        viaRoute.route("/custom").method("REPORT", "/ignored", (Action<Request, Response>)(static (_, res) => res.send("route")));
        await assertRoute(viaRoute, "REPORT", "/custom", "route");
    }

    [Fact]
    public async Task route_convenience_overloads_chain_and_execute()
    {
        var app = express.create();
        app.route("/chain/all").all((Action<Request, Response>)(static (_, res) => res.send("all")));
        app.route("/chain/delete").delete((Action<Request, Response>)(static (_, res) => res.send("delete")));
        app.route("/chain/get").get((Action<Request, Response>)(static (_, res) => res.send("get")));
        app.route("/chain/options").options((Action<Request, Response>)(static (_, res) => res.send("options")));
        app.route("/chain/patch").patch((Action<Request, Response>)(static (_, res) => res.send("patch")));
        app.route("/chain/post").post((Action<Request, Response>)(static (_, res) => res.send("post")));
        app.route("/chain/put").put((Action<Request, Response>)(static (_, res) => res.send("put")));

        await assertRoute(app, "GET", "/chain/all", "all");
        await assertRoute(app, "DELETE", "/chain/delete", "delete");
        await assertRoute(app, "GET", "/chain/get", "get");
        await assertRoute(app, "OPTIONS", "/chain/options", "options");
        await assertRoute(app, "PATCH", "/chain/patch", "patch");
        await assertRoute(app, "POST", "/chain/post", "post");
        await assertRoute(app, "PUT", "/chain/put", "put");
    }

    [Fact]
    public async Task routing_path_matching_covers_regex_arrays_splats_params_and_middleware_prefix()
    {
        var app = express.create();
        app.get(new Regex("^/rx/[0-9]+$"), (Action<Request, Response>)(static (_, res) => res.send("regex")));
        app.get(new object[] { "/array-a", "/array-b" }, (Action<Request, Response>)(static (_, res) => res.send("array")));
        app.use("/api/{*splat}", static (Request _, Response res, NextFunction next) =>
        {
            res.set("x-splat", "on");
            return next(null);
        });
        app.get("/api/:id", static (Request req, Response res) =>
        {
            var id = req.@params["id"] ?? string.Empty;
            res.send(id);
        });
        app.use("/prefix", static (Request _, Response res, NextFunction next) =>
        {
            res.set("x-prefix", "1");
            return next(null);
        });
        app.get("/prefix/test", (Action<Request, Response>)(static (_, res) => res.send("prefix")));

        await assertRoute(app, "GET", "/rx/42", "regex");
        await assertRoute(app, "GET", "/array-a", "array");
        await assertRoute(app, "GET", "/array-b", "array");

        var apiContext = test_runtime_utils.createContext("GET", "/api/77");
        await test_runtime_utils.run(app, apiContext);
        Assert.Equal("77", test_runtime_utils.readBody(apiContext));
        Assert.Equal("on", apiContext.Response.Headers["x-splat"].ToString());

        var prefixContext = test_runtime_utils.createContext("GET", "/prefix/test");
        await test_runtime_utils.run(app, prefixContext);
        Assert.Equal("prefix", test_runtime_utils.readBody(prefixContext));
        Assert.Equal("1", prefixContext.Response.Headers["x-prefix"].ToString());
    }

    [Fact]
    public async Task control_flow_and_handler_flattening_cover_route_router_and_low_parameter_handlers()
    {
        var app = express.create();

        app.get("/next-route",
            (RequestHandler)(async (_, _, next) => await next("route")),
            (Action<Request, Response>)(static (_, res) => res.send("wrong")));
        app.get("/next-route", (Action<Request, Response>)(static (_, res) => res.send("right")));

        var router = express.Router();
        router.get("/next-router", (RequestHandler)(async (_, _, next) => await next("router")));
        router.get("/next-router", (Action<Request, Response>)(static (_, res) => res.send("wrong-router")));
        app.use("/api", router);
        app.get("/api/next-router", (Action<Request, Response>)(static (_, res) => res.send("right-router")));

        app.use("/flat",
            "noop-string-handler",
            new object[]
            {
                (RequestHandler)((_, res, next) =>
                {
                    res.set("x-flat", "1");
                    return next(null);
                })
            },
            static (Request _, Response res) => res.send("flat"));

        app.use((Action)(() => { }));
        app.get("/zero-arg", (Action<Request, Response>)(static (_, res) => res.send("ok")));

        await assertRoute(app, "GET", "/next-route", "right");
        var nextRouterContext = test_runtime_utils.createContext("GET", "/api/next-router");
        await test_runtime_utils.run(app, nextRouterContext);
        Assert.Equal(string.Empty, test_runtime_utils.readBody(nextRouterContext));

        var flatContext = test_runtime_utils.createContext("GET", "/flat");
        await test_runtime_utils.run(app, flatContext);
        Assert.Equal("flat", test_runtime_utils.readBody(flatContext));
        Assert.Equal("1", flatContext.Response.Headers["x-flat"].ToString());

        await assertRoute(app, "GET", "/zero-arg", "ok");
    }

    [Fact]
    public async Task error_handling_and_param_callbacks_cover_once_per_value_and_four_arg_handlers()
    {
        var app = express.create();
        var count = 0;
        app.param("id", async (_, _, next, _, _) =>
        {
            count++;
            await next(null);
        });

        app.get("/users/:id", (RequestHandler)(async (_, _, next) => await next(null)));
        app.get("/users/:id", (Action<Request, Response>)(static (_, res) => res.send("done")));

        await assertRoute(app, "GET", "/users/42", "done");
        Assert.Equal(1, count);

        app.get("/boom", (RequestHandler)(static (_, _, _) => throw new InvalidOperationException("boom")));
        app.use((Func<Exception, Request, Response, NextFunction, Task>)(static (_, __, res, ___) =>
        {
            res.status(500).send("handled");
            return Task.CompletedTask;
        }));

        await assertRoute(app, "GET", "/boom", "handled", expectedStatus: 500);
    }

    [Fact]
    public void router_export_combines_mount_paths_and_handles_non_string_mount_path()
    {
        var child = express.Router();
        child.get("/child", (Action<Request, Response>)(static (_, res) => res.send("child")));
        child.get("/", (Action<Request, Response>)(static (_, res) => res.send("root")));

        var combined = child.export("/api");
        Assert.Contains(combined, layer => string.Equals(layer.path?.ToString(), "/api/child", StringComparison.Ordinal));
        Assert.Contains(combined, layer => string.Equals(layer.path?.ToString(), "/api", StringComparison.Ordinal));

        var regexMount = child.export(new Regex("^/rx"));
        Assert.Contains(regexMount, layer => string.Equals(layer.path?.ToString(), "/child", StringComparison.Ordinal));
    }

    [Fact]
    public async Task null_route_path_matches_all_requests()
    {
        var app = express.create();
        app.method("GET", null!, (Action<Request, Response>)(static (_, res) => res.send("null-path")));
        await assertRoute(app, "GET", "/anywhere", "null-path");
    }

    [Fact]
    public async Task router_use_with_mounted_application_and_additional_path_edge_cases()
    {
        var app = express.create();
        var router = express.Router();
        var child = express.create();
        child.get("/child", (Action<Request, Response>)(static (_, res) => res.send("child-app")));
        router.use("/mount", child);

        router.use("/skip-null", null!, (Action<Request, Response>)(static (_, res) => res.send("null-skipped")));

        app.use(router);
        await assertRoute(app, "GET", "/mount/child", "child-app");
        await assertRoute(app, "GET", "/skip-null", "null-skipped");
    }

    [Fact]
    public async Task unsupported_path_specs_and_colon_param_length_mismatch_do_not_match()
    {
        var app = express.create();
        app.get(123, (Action<Request, Response>)(static (_, res) => res.send("bad")));

        app.use("/a/:b/:c", (RequestHandler)(static (_, res, next) =>
        {
            res.set("x-should-not-match", "1");
            return next(null);
        }));
        app.get("/a/1", (Action<Request, Response>)(static (_, res) => res.send("ok")));

        var context = test_runtime_utils.createContext("GET", "/a/1");
        await test_runtime_utils.run(app, context);
        Assert.Equal("ok", test_runtime_utils.readBody(context));
        Assert.False(context.Response.Headers.ContainsKey("x-should-not-match"));
    }

    [Fact]
    public async Task normalize_path_handles_empty_and_missing_slash_specs()
    {
        var rootApp = express.create();
        rootApp.get("", (Action<Request, Response>)(static (_, res) => res.send("root")));
        await assertRoute(rootApp, "GET", "/", "root");

        var noSlashApp = express.create();
        noSlashApp.get("noslash", (Action<Request, Response>)(static (_, res) => res.send("noslash")));
        await assertRoute(noSlashApp, "GET", "/noslash", "noslash");
    }

    [Fact]
    public void export_preserves_non_string_paths_and_handles_root_mount_prefix()
    {
        var router = express.Router();
        router.get(new Regex("^/rx"), (Action<Request, Response>)(static (_, res) => res.send("rx")));
        router.get("/plain", (Action<Request, Response>)(static (_, res) => res.send("plain")));

        var rootMounted = router.export("/");
        Assert.Contains(rootMounted, layer => string.Equals(layer.path?.ToString(), "/plain", StringComparison.Ordinal));

        var regexMounted = router.export("/api");
        Assert.Contains(regexMounted, layer => layer.path is Regex);
    }

    [Fact]
    public async Task method_with_null_value_and_unsupported_delegate_handlers_fall_through()
    {
        var app = express.create();
        app.method(null!, "/null-method", (Action<Request, Response>)(static (_, res) => res.send("null-method")));
        await assertRoute(app, "GET", "/null-method", "null-method");

        app.get("/badinvoke", (object)(Func<int, int, int, int>)((_, _, _) => 1));
        app.get("/badinvoke", (Action<Request, Response>)(static (_, res) => res.send("fallback")));
        app.use((Func<Exception, Request, Response, NextFunction, Task>)(static (_, __, res, ___) =>
        {
            res.status(500).send("invoke-error");
            return Task.CompletedTask;
        }));

        await assertRoute(app, "GET", "/badinvoke", "fallback");
    }

    private static async Task assertRoute(Application app, string method, string path, string expectedBody, int expectedStatus = 200)
    {
        var context = test_runtime_utils.createContext(method, path);
        await test_runtime_utils.run(app, context);
        Assert.Equal(expectedStatus, context.Response.StatusCode);
        Assert.Equal(expectedBody, test_runtime_utils.readBody(context));
    }
}
