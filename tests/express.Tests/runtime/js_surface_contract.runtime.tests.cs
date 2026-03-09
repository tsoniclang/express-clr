using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using express;
using Microsoft.AspNetCore.Http;
using Tsonic.JSRuntime;
using Tsonic.Runtime;
using Xunit;

namespace express.Tests.runtime;

public class js_surface_contract_runtime_tests
{
    [Fact]
    public void numeric_js_boundaries_validate_integer_backing_types()
    {
        var urlencoded = new UrlEncodedOptions();
        urlencoded.parameterLimit = 32;
        urlencoded.depth = 4;

        var multipartField = new MultipartField { maxCount = 1 };
        var multipartOptions = new MultipartOptions
        {
            maxFileCount = 2,
            maxFileSizeBytes = 4096,
        };

        var cors = new CorsOptions
        {
            maxAgeSeconds = 60,
            optionsSuccessStatus = 204,
        };

        var response = new Response();
        response.statusCode = 201;

        Assert.Throws<ArgumentException>(() => urlencoded.parameterLimit = 1.5);
        Assert.Throws<ArgumentException>(() => multipartField.maxCount = 1.25);
        Assert.Throws<ArgumentException>(() => multipartOptions.maxFileCount = 2.5);
        Assert.Throws<ArgumentOutOfRangeException>(() => multipartOptions.maxFileSizeBytes = double.PositiveInfinity);
        Assert.Throws<ArgumentException>(() => cors.optionsSuccessStatus = 204.5);
        Assert.Throws<ArgumentException>(() => response.statusCode = 200.1);
    }

    [Fact]
    public void js_date_backed_options_and_metadata_round_trip_as_js_dates()
    {
        var cookie = new CookieOptions
        {
            expires = new Date(0),
            maxAge = 30_000,
        };

        var stat = new FileStat
        {
            size = 128,
            modifiedAt = new Date(1_700_000_000_000),
        };

        Assert.Equal(0, cookie.expires!.valueOf());
        Assert.Equal(30_000, cookie.maxAge);
        Assert.Equal(128, stat.size);
        Assert.Equal("2023-11-14T22:13:20.000Z", stat.modifiedAt.toISOString());
    }

    [Fact]
    public async Task uploaded_file_js_helpers_round_trip_bytes_text_and_save()
    {
        var bytes = Encoding.UTF8.GetBytes("hello from express");
        await using var stream = new MemoryStream(bytes);
        IFormFile formFile = new FormFile(stream, 0, bytes.Length, "avatar", "avatar.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain",
        };

        var uploaded = new UploadedFile(formFile);

        var payload = await uploaded.bytes();
        var text = await uploaded.text();

        Assert.Equal(bytes.Length, uploaded.size);
        Assert.Equal("avatar", uploaded.fieldname);
        Assert.Equal("avatar.txt", uploaded.originalname);
        Assert.Equal("text/plain", uploaded.mimetype);
        Assert.Equal(bytes, payload.ToArray());
        Assert.Equal("hello from express", text);

        var outputPath = Path.Combine(Path.GetTempPath(), $"express-upload-{Guid.NewGuid():N}.txt");
        try
        {
            await uploaded.save(outputPath);
            Assert.Equal("hello from express", File.ReadAllText(outputPath));
        }
        finally
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }
    }

    [Fact]
    public void request_range_and_error_callbacks_follow_js_surface_contracts()
    {
        Union<RangeResult, int> range = new Request().range(1024);

        Assert.True(range != -1);
        Assert.Equal(0, range.As1().ranges[0].start);
        Assert.Equal(1023, range.As1().ranges[0].end);

        var server = new AppServer(1, "127.0.0.1", null, () => throw new InvalidOperationException("close"));
        Error? callbackError = null;
        server.close(err => callbackError = err);

        Assert.NotNull(callbackError);
        Assert.Equal("close", callbackError!.message);
        Assert.IsType<InvalidOperationException>(callbackError.InnerException);
    }
}
