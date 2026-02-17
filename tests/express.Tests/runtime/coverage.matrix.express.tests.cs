using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using express;
using Xunit;

namespace express.Tests.runtime;

public class coverage_matrix_express_tests
{
    [Fact]
    public void top_level_factories_and_router_return_expected_types()
    {
        Assert.IsType<Application>(express.create());
        Assert.IsType<Application>(express.application());
        Assert.IsType<Application>(express.app());
        Assert.IsType<Router>(express.Router(new RouterOptions { caseSensitive = true }));
    }

    [Fact]
    public async Task json_type_matrix_covers_default_string_array_matcher_and_fallback()
    {
        var defaultJson = await parseWithJson(null, "application/json", "{\"ok\":true}", null);
        Assert.Contains("\"ok\":true", defaultJson);

        var customString = await parseWithJson("application/custom+json", "application/custom+json", "{\"k\":1}", null);
        Assert.Contains("\"k\":1", customString);

        var customArray = await parseWithJson(new[] { "application/vnd.api+json" }, "application/vnd.api+json", "{\"v\":2}", null);
        Assert.Contains("\"v\":2", customArray);

        var nonMatchingArray = await parseWithJson(new[] { "application/nope" }, "text/plain", "{\"n\":5}", null);
        Assert.Equal("null", nonMatchingArray);

        var customMatcher = await parseWithJson(
            (MediaTypeMatcher)(req => req.get("x-parse") == "1"),
            "text/plain",
            "{\"m\":3}",
            "1");
        Assert.Contains("\"m\":3", customMatcher);

        var fallbackToDefault = await parseWithJson(new object(), "application/json", "{\"f\":4}", null);
        Assert.Contains("\"f\":4", fallbackToDefault);
    }

    [Fact]
    public async Task parsers_skip_when_content_type_does_not_match()
    {
        var app = express.create();
        app.use(express.json());
        app.use(express.raw(new RawOptions { type = "application/custom" }));
        app.use(express.text(new TextOptions { type = "text/custom" }));
        app.use(express.urlencoded(new UrlEncodedOptions { type = "application/x-custom-form" }));
        app.post("/skip", static (Request req, Response res, NextFunction _next) =>
        {
            res.send(req.body is null ? "null" : "not-null");
            return Task.CompletedTask;
        });

        var context = test_runtime_utils.createContext("POST", "/skip", "a=1&a=2", "application/x-www-form-urlencoded");
        await test_runtime_utils.run(app, context);
        Assert.Equal("null", test_runtime_utils.readBody(context));
    }

    [Fact]
    public async Task json_skips_when_content_type_is_missing_or_mismatched()
    {
        var app = express.create();
        app.use(express.json());
        app.post("/json", static (Request req, Response res, NextFunction _next) =>
        {
            res.send(req.body is null ? "null" : "not-null");
            return Task.CompletedTask;
        });

        var missingContentType = test_runtime_utils.createContext("POST", "/json", "{\"a\":1}");
        await test_runtime_utils.run(app, missingContentType);
        Assert.Equal("null", test_runtime_utils.readBody(missingContentType));

        var mismatchedContentType = test_runtime_utils.createContext("POST", "/json", "{\"a\":1}", "text/plain");
        await test_runtime_utils.run(app, mismatchedContentType);
        Assert.Equal("null", test_runtime_utils.readBody(mismatchedContentType));
    }

    [Fact]
    public async Task urlencoded_parser_converts_multivalue_keys_to_arrays()
    {
        var app = express.create();
        app.use(express.urlencoded());
        app.post("/form", static (Request req, Response res, NextFunction _next) =>
        {
            var body = Assert.IsType<Dictionary<string, object?>>(req.body);
            var values = Assert.IsType<string[]>(body["a"]);
            res.send($"{values[0]}-{values[1]}");
            return Task.CompletedTask;
        });

        var context = test_runtime_utils.createContext("POST", "/form", "a=1&a=2", "application/x-www-form-urlencoded");
        await test_runtime_utils.run(app, context);
        Assert.Equal("1-2", test_runtime_utils.readBody(context));
    }

    [Fact]
    public async Task parser_middleware_handles_requests_without_http_context()
    {
        var req = new Request();
        req.setHeader("Content-Type", "application/json");
        var res = new Response();
        var middleware = express.json();

        await middleware(req, res, static _ => Task.CompletedTask);

        Assert.Null(req.body);
    }

    [Fact]
    public async Task static_middleware_serves_index_and_falls_through_when_missing()
    {
        var root = Path.Combine(Path.GetTempPath(), $"express-static-{System.Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, "index.html"), "home");
        File.WriteAllText(Path.Combine(root, "asset.txt"), "asset");

        try
        {
            var app = express.create();
            app.use(express.@static(root, new StaticOptions { index = "index.html" }));
            app.get("/{*splat}", static (Request _req, Response res, NextFunction _next) =>
            {
                res.send("next");
                return Task.CompletedTask;
            });

            var indexContext = test_runtime_utils.createContext("GET", "/");
            await test_runtime_utils.run(app, indexContext);
            Assert.Equal("home", test_runtime_utils.readBody(indexContext));

            var fileContext = test_runtime_utils.createContext("GET", "/asset.txt");
            await test_runtime_utils.run(app, fileContext);
            Assert.Equal("asset", test_runtime_utils.readBody(fileContext));

            var missContext = test_runtime_utils.createContext("GET", "/missing.txt");
            await test_runtime_utils.run(app, missContext);
            Assert.Equal("next", test_runtime_utils.readBody(missContext));

            var appWithoutStringIndex = express.create();
            appWithoutStringIndex.use(express.@static(root, new StaticOptions { index = false }));
            appWithoutStringIndex.get("/{*splat}", static (Request _req, Response res, NextFunction _next) =>
            {
                res.send("next");
                return Task.CompletedTask;
            });
            var rootFallbackContext = test_runtime_utils.createContext("GET", "/");
            await test_runtime_utils.run(appWithoutStringIndex, rootFallbackContext);
            Assert.Equal("next", test_runtime_utils.readBody(rootFallbackContext));

            var appWithDefaultIndex = express.create();
            appWithDefaultIndex.use(express.@static(root));
            appWithDefaultIndex.get("/{*splat}", static (Request _req, Response res, NextFunction _next) =>
            {
                res.send("next");
                return Task.CompletedTask;
            });
            var defaultIndexFallbackContext = test_runtime_utils.createContext("GET", "/");
            await test_runtime_utils.run(appWithDefaultIndex, defaultIndexFallbackContext);
            Assert.Equal("next", test_runtime_utils.readBody(defaultIndexFallbackContext));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    private static async Task<string> parseWithJson(object? configuredType, string contentType, string payload, string? parseHeaderValue)
    {
        var app = express.create();
        var options = new JsonOptions { type = configuredType };
        app.use(express.json(options));
        app.post("/json", static (Request req, Response res, NextFunction _next) =>
        {
            res.json(req.body);
            return Task.CompletedTask;
        });

        var context = test_runtime_utils.createContext("POST", "/json", payload, contentType);
        if (parseHeaderValue is not null)
            context.Request.Headers["x-parse"] = parseHeaderValue;
        await test_runtime_utils.run(app, context);
        return test_runtime_utils.readBody(context);
    }
}
