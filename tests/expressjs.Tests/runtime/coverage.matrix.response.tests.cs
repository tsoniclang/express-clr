using System;
using System.Collections.Generic;
using System.IO;
using expressjs;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace expressjs.Tests.runtime;

public class coverage_matrix_response_tests
{
    [Fact]
    public void response_exposes_app_and_locals_and_get_falls_back_to_http_context_headers()
    {
        var app = express.create();
        var context = test_runtime_utils.createContext("GET", "/");
        context.Response.Headers["x-fallback"] = "ok";
        var req = Request.fromHttpContext(context, app);
        var res = Response.fromHttpContext(context, req);

        res.locals["name"] = "value";

        Assert.Same(app, res.app);
        Assert.Equal("value", res.locals["name"]);
        Assert.Equal("ok", res.get("x-fallback"));
    }

    [Fact]
    public void append_attachment_cookie_and_clear_cookie_cover_core_header_behaviors()
    {
        var res = new Response();

        res.append("Warning", "199 misc").append("Warning", "299 extra");
        res.append("Link", new[] { "<a>", "<b>" });
        Assert.Contains("199 misc", res.get("Warning"));
        Assert.Contains("299 extra", res.get("Warning"));
        Assert.Contains("<a>", res.get("Link"));
        Assert.Contains("<b>", res.get("Link"));

        res.attachment();
        Assert.Equal("attachment", res.get("Content-Disposition"));

        res.attachment("logo.png");
        Assert.Contains("filename=\"logo.png\"", res.get("Content-Disposition"));
        Assert.Equal("image/png", res.get("Content-Type"));

        res.cookie("obj", new Dictionary<string, object?> { ["a"] = 1 }, new CookieOptions { encode = static value => $"enc:{value}" });
        Assert.Contains("obj=enc:", res.get("Set-Cookie"));

        res.clearCookie("obj", new CookieOptions { path = "/admin" });
        Assert.Contains("obj=", res.get("Set-Cookie"));
        res.clearCookie("obj");
    }

    [Fact]
    public void format_covers_empty_default_and_first_handler_paths()
    {
        var res = new Response();
        res.format(new Dictionary<string, Action>());

        var used = "";
        res.format(new Dictionary<string, Action>
        {
            ["default"] = () => used = "default",
            ["json"] = () => used = "json"
        });
        Assert.Equal("default", used);

        used = "";
        res.format(new Dictionary<string, Action>
        {
            ["json"] = () => used = "json",
            ["text"] = () => used = "text"
        });
        Assert.Equal("json", used);
    }

    [Fact]
    public void json_jsonp_links_location_redirect_set_status_type_and_vary_work()
    {
        var app = express.create();
        app.set("jsonp callback name", "cb");
        var req = new Request { app = app };
        var res = new Response { req = req };

        res.json(null);
        Assert.Equal("application/json", res.get("Content-Type"));

        res.json("raw-json");
        Assert.Equal("application/json", res.get("Content-Type"));

        res.json(new Dictionary<string, object?> { ["ok"] = true });
        Assert.Equal("application/json", res.get("Content-Type"));

        res.jsonp("raw-jsonp");
        res.jsonp(new Dictionary<string, object?> { ["ok"] = true });
        Assert.Equal("application/javascript", res.get("Content-Type"));

        var jsonpContext = test_runtime_utils.createContext("GET", "/");
        var jsonpRequest = Request.fromHttpContext(jsonpContext, app);
        var jsonpResponse = Response.fromHttpContext(jsonpContext, jsonpRequest);
        jsonpResponse.jsonp(new Dictionary<string, object?> { ["ok"] = true });
        Assert.StartsWith("cb(", test_runtime_utils.readBody(jsonpContext));

        res.links(new Dictionary<string, string> { ["next"] = "http://a", ["last"] = "http://b" });
        Assert.Contains("rel=\"next\"", res.get("Link"));
        Assert.Contains("rel=\"last\"", res.get("Link"));

        res.location("/admin");
        Assert.Equal("/admin", res.get("Location"));

        res.redirect("/moved");
        Assert.Equal(302, res.statusCode);
        Assert.Equal("/moved", res.get("Location"));

        res.redirect(301, "/permanent");
        Assert.Equal(301, res.statusCode);
        Assert.Equal("/permanent", res.get("Location"));

        res.set("x-one", "1");
        res.set(new Dictionary<string, string> { ["x-two"] = "2" });
        res.header("x-three", "3");
        Assert.Equal("1", res.get("x-one"));
        Assert.Equal("2", res.get("x-two"));
        Assert.Equal("3", res.get("x-three"));

        res.status(418).type("json").contentType("text/plain").vary("Accept").vary("User-Agent");
        Assert.Equal(418, res.statusCode);
        Assert.Equal("text/plain", res.get("Content-Type"));
        Assert.Equal("Accept, User-Agent", res.get("Vary"));

        var context = test_runtime_utils.createContext("GET", "/");
        var contextReq = Request.fromHttpContext(context, app);
        var contextRes = Response.fromHttpContext(context, contextReq);
        contextRes.redirect(307, "/ctx");
        Assert.Equal("/ctx", context.Response.Headers.Location.ToString());
        contextRes.vary("Accept-Encoding");
        Assert.Equal("Accept-Encoding", context.Response.Headers["Vary"].ToString());

        var noAppResponse = new Response();
        noAppResponse.jsonp("x");
        Assert.Equal("application/javascript", noAppResponse.get("Content-Type"));
    }

    [Fact]
    public void render_overloads_use_engine_when_available_or_fallback_when_missing()
    {
        var app = express.create();
        app.engine("tpl", static (_, locals, callback) => callback(null, $"hello {locals["name"]}"));

        var req = new Request { app = app };
        var res = new Response { req = req };
        res.locals["name"] = "local";

        res.render("home.tpl");
        Assert.True(res.headersSent);

        var withLocals = new Response { req = req };
        withLocals.render("home.tpl", new Dictionary<string, object?> { ["name"] = "world" });
        Assert.True(withLocals.headersSent);

        string? callbackHtml = null;
        res.render("home.tpl", (_, html) => callbackHtml = html);
        Assert.Equal("hello local", callbackHtml);

        string? callbackHtmlWithLocals = null;
        res.render("home.tpl", new Dictionary<string, object?> { ["name"] = "matrix" }, (_, html) => callbackHtmlWithLocals = html);
        Assert.Equal("hello matrix", callbackHtmlWithLocals);

        var fallback = new Response { req = req };
        string? fallbackHtml = null;
        fallback.render("missing.engine", (_, html) => fallbackHtml = html);
        Assert.Equal("<rendered:missing.engine>", fallbackHtml);

        var fallbackWithLocals = new Response { req = req };
        fallbackWithLocals.render("missing.engine", new Dictionary<string, object?> { ["name"] = "x" });
        Assert.True(fallbackWithLocals.headersSent);

        string? fallbackWithLocalsCallback = null;
        fallback.render("missing.engine", new Dictionary<string, object?> { ["name"] = "x" }, (_, html) => fallbackWithLocalsCallback = html);
        Assert.Equal("<rendered:missing.engine>", fallbackWithLocalsCallback);

        app.engine("tplnull", static (_, _, callback) => callback(null, null));
        var nullHtmlResponse = new Response { req = req };
        nullHtmlResponse.render("null.tplnull");
        Assert.True(nullHtmlResponse.headersSent);
        nullHtmlResponse.render("null.tplnull", new Dictionary<string, object?> { ["name"] = "x" });
        Assert.True(nullHtmlResponse.headersSent);

        var noAppResponse = new Response();
        noAppResponse.render("noapp.tpl", new Dictionary<string, object?> { ["name"] = "x" });
        Assert.True(noAppResponse.headersSent);
        string? noAppCallback = null;
        noAppResponse.render("noapp.tpl", (_, html) => noAppCallback = html);
        Assert.Equal("<rendered:noapp.tpl>", noAppCallback);
        string? noAppLocalsCallback = null;
        noAppResponse.render("noapp.tpl", new Dictionary<string, object?> { ["name"] = "x" }, (_, html) => noAppLocalsCallback = html);
        Assert.Equal("<rendered:noapp.tpl>", noAppLocalsCallback);
    }

    [Fact]
    public void send_end_sendstatus_sendfile_and_download_cover_success_and_error_paths()
    {
        var context = test_runtime_utils.createContext("GET", "/");
        var req = Request.fromHttpContext(context, express.create());
        var res = Response.fromHttpContext(context, req);

        res.send("text");
        Assert.Equal("text", test_runtime_utils.readBody(context));
        Assert.True(res.headersSent);

        var binaryContext = test_runtime_utils.createContext("GET", "/");
        var binaryRes = Response.fromHttpContext(binaryContext, Request.fromHttpContext(binaryContext, express.create()));
        binaryRes.send(new byte[] { 1, 2, 3 });
        Assert.Equal("application/octet-stream", binaryRes.get("Content-Type"));

        var objectContext = test_runtime_utils.createContext("GET", "/");
        var objectRes = Response.fromHttpContext(objectContext, Request.fromHttpContext(objectContext, express.create()));
        objectRes.send(new Dictionary<string, object?> { ["value"] = 1 });
        Assert.Equal("application/json", objectRes.get("Content-Type"));

        var nullContext = test_runtime_utils.createContext("GET", "/");
        var nullRes = Response.fromHttpContext(nullContext, Request.fromHttpContext(nullContext, express.create()));
        nullRes.send();
        Assert.Equal(string.Empty, test_runtime_utils.readBody(nullContext));

        var ended = false;
        var endContext = test_runtime_utils.createContext("GET", "/");
        var endRes = Response.fromHttpContext(endContext, Request.fromHttpContext(endContext, express.create()));
        endRes.status(204).end("done", callback: () => ended = true);
        Assert.True(ended);
        Assert.True(endRes.headersSent);

        var statusContext = test_runtime_utils.createContext("GET", "/");
        var statusRes = Response.fromHttpContext(statusContext, Request.fromHttpContext(statusContext, express.create()));
        statusRes.sendStatus(404);
        Assert.Equal(404, statusRes.statusCode);
        Assert.Equal("404", test_runtime_utils.readBody(statusContext));

        var root = Path.Combine(Path.GetTempPath(), $"expressjs-files-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, "a.txt"), "A");
        File.WriteAllText(Path.Combine(root, "b.txt"), "B");

        try
        {
            var sendContext = test_runtime_utils.createContext("GET", "/");
            var sendRes = Response.fromHttpContext(sendContext, Request.fromHttpContext(sendContext, express.create()));
            sendRes.sendFile("a.txt", new SendFileOptions { root = root });
            Assert.Equal("A", test_runtime_utils.readBody(sendContext));

            var absoluteContext = test_runtime_utils.createContext("GET", "/");
            var absoluteRes = Response.fromHttpContext(absoluteContext, Request.fromHttpContext(absoluteContext, express.create()));
            absoluteRes.sendFile(Path.Combine(root, "b.txt"));
            Assert.Equal("B", test_runtime_utils.readBody(absoluteContext));

            var relativeName = $"expressjs-relative-{Guid.NewGuid():N}.txt";
            File.WriteAllText(relativeName, "R");
            try
            {
                var relativeContext = test_runtime_utils.createContext("GET", "/");
                var relativeRes = Response.fromHttpContext(relativeContext, Request.fromHttpContext(relativeContext, express.create()));
                Exception? relativeError = null;
                relativeRes.sendFile(relativeName, fn: err => relativeError = err);
                Assert.Null(relativeError);
                Assert.Equal("R", test_runtime_utils.readBody(relativeContext));
            }
            finally
            {
                File.Delete(relativeName);
            }

            Exception? sendError = null;
            sendRes.sendFile("missing.txt", new SendFileOptions { root = root }, err => sendError = err);
            Assert.NotNull(sendError);

            var downloadContext = test_runtime_utils.createContext("GET", "/");
            var downloadRes = Response.fromHttpContext(downloadContext, Request.fromHttpContext(downloadContext, express.create()));
            Exception? downloadError = null;
            downloadRes.download(Path.Combine(root, "b.txt"), fn: err => downloadError = err);
            Assert.Null(downloadError);
            Assert.Equal("B", test_runtime_utils.readBody(downloadContext));

            Exception? downloadWithOptionsError = null;
            downloadRes.download(Path.Combine(root, "b.txt"), options: new DownloadOptions { root = root }, fn: err => downloadWithOptionsError = err);
            Assert.Null(downloadWithOptionsError);

            Exception? downloadMissingError = null;
            downloadRes.download(Path.Combine(root, "missing.txt"), fn: err => downloadMissingError = err);
            Assert.Null(downloadMissingError);

            Exception? downloadCatchError = null;
            downloadRes.download(Path.Combine(root, "b.txt"), fn: err =>
            {
                if (err is null)
                    throw new InvalidOperationException("force callback failure");
                downloadCatchError = err;
            });
            Assert.NotNull(downloadCatchError);

            downloadRes.set("x-null", null);
            Assert.Equal(string.Empty, downloadRes.get("x-null"));
            downloadRes.type("unknownext");
            Assert.Equal("application/octet-stream", downloadRes.get("Content-Type"));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }
}
