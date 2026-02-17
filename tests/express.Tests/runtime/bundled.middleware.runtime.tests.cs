using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using express;
using Xunit;

namespace express.Tests.runtime;

public class bundled_middleware_runtime_tests
{
    [Fact]
    public async Task cookie_parser_moves_valid_signed_cookies_to_signedCookies()
    {
        var secret = "shh";
        var signed = CookieSignature.sign("abc", secret);

        var app = express.create();
        app.use(express.cookieParser(secret));
        app.get("/signed", static (Request req, Response res, NextFunction _) =>
        {
            var payload = new Dictionary<string, object?>
            {
                ["signed"] = req.signedCookies["sid"],
                ["cookie"] = req.cookies["sid"],
            };
            res.json(payload);
            return Task.CompletedTask;
        });

        var context = test_runtime_utils.createContext("GET", "/signed");
        context.Request.Headers["Cookie"] = $"sid={signed}";
        await test_runtime_utils.run(app, context);

        var body = test_runtime_utils.readBody(context);
        Assert.Contains("\"signed\":\"abc\"", body);
        Assert.Contains("\"cookie\":null", body);
    }

    [Fact]
    public async Task response_cookie_signs_when_cookie_parser_secret_is_installed()
    {
        var secret = "shh";

        var app = express.create();
        app.use(express.cookieParser(secret));
        app.get("/set", static (Request _req, Response res, NextFunction _next) =>
        {
            res.cookie("sid", "abc", new CookieOptions { signed = true });
            res.send("ok");
            return Task.CompletedTask;
        });

        var context = test_runtime_utils.createContext("GET", "/set");
        await test_runtime_utils.run(app, context);

        var header = context.Response.Headers["Set-Cookie"].ToString();
        Assert.Contains("sid=s:abc.", header);
    }

    [Fact]
    public void response_cookie_throws_when_signing_without_secret()
    {
        var res = new Response
        {
            req = new Request { app = express.create() }
        };

        Assert.Throws<InvalidOperationException>(() =>
            res.cookie("sid", "abc", new CookieOptions { signed = true }));
    }

    [Fact]
    public async Task cors_sets_allow_origin_for_simple_requests_and_skips_when_origin_missing()
    {
        var app = express.create();
        app.use(express.cors());
        app.get("/x", static (_req, res, _next) =>
        {
            res.send("ok");
            return Task.CompletedTask;
        });

        var missingOrigin = test_runtime_utils.createContext("GET", "/x");
        await test_runtime_utils.run(app, missingOrigin);
        Assert.False(missingOrigin.Response.Headers.ContainsKey("Access-Control-Allow-Origin"));

        var withOrigin = test_runtime_utils.createContext("GET", "/x");
        withOrigin.Request.Headers["Origin"] = "https://example.com";
        await test_runtime_utils.run(app, withOrigin);
        Assert.Equal("*", withOrigin.Response.Headers["Access-Control-Allow-Origin"].ToString());
    }

    [Fact]
    public async Task cors_handles_preflight_and_respects_preflight_continue()
    {
        var app = express.create();
        app.use(express.cors());
        app.options("/x", static (_req, res, _next) =>
        {
            res.send("should-not-run");
            return Task.CompletedTask;
        });

        var preflight = test_runtime_utils.createContext("OPTIONS", "/x");
        preflight.Request.Headers["Origin"] = "https://example.com";
        preflight.Request.Headers["Access-Control-Request-Method"] = "POST";
        await test_runtime_utils.run(app, preflight);
        Assert.Equal(204, preflight.Response.StatusCode);
        Assert.Equal("POST", preflight.Response.Headers["Access-Control-Allow-Methods"].ToString());

        var withContinue = express.create();
        withContinue.use(express.cors(new CorsOptions { preflightContinue = true }));
        withContinue.options("/x", static (_req, res, _next) =>
        {
            res.send("ran");
            return Task.CompletedTask;
        });

        var continued = test_runtime_utils.createContext("OPTIONS", "/x");
        continued.Request.Headers["Origin"] = "https://example.com";
        continued.Request.Headers["Access-Control-Request-Method"] = "POST";
        await test_runtime_utils.run(withContinue, continued);
        Assert.Equal("ran", test_runtime_utils.readBody(continued));
    }

    [Fact]
    public async Task multipart_parses_fields_and_files_for_single_upload()
    {
        var app = express.create();
        var upload = express.multipart();
        app.use(upload.single("avatar"));
        app.post("/upload", static (Request req, Response res, NextFunction _) =>
        {
            var body = req.body as Dictionary<string, object?>;
            var payload = new Dictionary<string, object?>
            {
                ["title"] = body?["title"],
                ["file"] = req.file?.originalname,
                ["count"] = req.files["avatar"]?.Length ?? 0
            };
            res.json(payload);
            return Task.CompletedTask;
        });

        var boundary = "----tsonic-test";
        var multipartBody =
            $"--{boundary}\r\n" +
            "Content-Disposition: form-data; name=\"title\"\r\n\r\n" +
            "hello\r\n" +
            $"--{boundary}\r\n" +
            "Content-Disposition: form-data; name=\"avatar\"; filename=\"a.txt\"\r\n" +
            "Content-Type: text/plain\r\n\r\n" +
            "file\r\n" +
            $"--{boundary}--\r\n";

        var context = test_runtime_utils.createContext("POST", "/upload", multipartBody, $"multipart/form-data; boundary={boundary}");
        await test_runtime_utils.run(app, context);

        var bodyText = test_runtime_utils.readBody(context);
        Assert.Contains("\"title\":\"hello\"", bodyText);
        Assert.Contains("\"file\":\"a.txt\"", bodyText);
        Assert.Contains("\"count\":1", bodyText);
    }

    [Fact]
    public async Task multipart_single_throws_for_unexpected_file_field()
    {
        var app = express.create();
        var context = test_runtime_utils.createContext("POST", "/ignored", "", "multipart/form-data; boundary=x");
        var req = Request.fromHttpContext(context, app);
        var res = Response.fromHttpContext(context, req);

        var boundary = "x";
        var multipartBody =
            $"--{boundary}\r\n" +
            "Content-Disposition: form-data; name=\"other\"; filename=\"a.txt\"\r\n" +
            "Content-Type: text/plain\r\n\r\n" +
            "file\r\n" +
            $"--{boundary}--\r\n";

        context.Request.Body = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(multipartBody));
        context.Request.ContentType = $"multipart/form-data; boundary={boundary}";

        var middleware = express.multipart().single("avatar");
        await Assert.ThrowsAsync<InvalidOperationException>(() => middleware(req, res, static _ => Task.CompletedTask));
    }
}
