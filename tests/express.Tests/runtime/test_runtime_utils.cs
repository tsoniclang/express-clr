using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace express.Tests.runtime;

internal static class test_runtime_utils
{
    public static DefaultHttpContext createContext(
        string method,
        string path,
        string? body = null,
        string? contentType = null,
        string? queryString = null,
        string? host = null)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Scheme = "http";
        context.Request.Path = path;
        context.Request.QueryString = string.IsNullOrWhiteSpace(queryString) ? QueryString.Empty : new QueryString(queryString);
        if (!string.IsNullOrWhiteSpace(host))
            context.Request.Host = HostString.FromUriComponent(host);
        context.Connection.RemoteIpAddress = IPAddress.Loopback;
        context.Response.Body = new MemoryStream();

        if (body is not null)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            context.Request.Body = new MemoryStream(bytes);
            context.Request.ContentType = contentType;
        }

        return context;
    }

    public static string readBody(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true);
        return reader.ReadToEnd();
    }

    public static async Task run(Application app, DefaultHttpContext context)
    {
        await app.handle(context, app);
    }

    public static int reserveTcpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
