using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using expressjs;
using Xunit;

namespace expressjs.Tests.surface;

public class express_api_surface_tests
{
    [Fact]
    public void express_module_exposes_expected_top_level_members()
    {
        var type = typeof(express);

        assertMethod(type, "create");
        assertMethod(type, "application");
        assertMethod(type, "app");
        assertMethod(type, "json");
        assertMethod(type, "raw");
        assertMethod(type, "Router");
        assertMethod(type, "static");
        assertMethod(type, "text");
        assertMethod(type, "urlencoded");
    }

    [Fact]
    public void application_exposes_expected_properties_events_and_methods()
    {
        var type = typeof(Application);

        assertProperty(type, "locals");
        assertProperty(type, "mountpath");
        assertProperty(type, "router");
        assertEvent(type, "mount");

        var methods = new[]
        {
            "all", "delete", "disable", "disabled", "enable", "enabled",
            "engine", "get", "listen", "method", "param", "path", "post",
            "put", "render", "route", "set", "use"
        };

        foreach (var method in methods)
            assertMethod(type, method);

        var routingMethods = new[]
        {
            "checkout", "copy", "head", "lock_", "merge", "mkactivity", "mkcol",
            "move", "m_search", "notify", "options", "patch", "purge", "report",
            "search", "subscribe", "trace", "unlock", "unsubscribe"
        };
        foreach (var method in routingMethods)
            assertMethod(type, method);
    }

    [Fact]
    public void request_exposes_expected_properties_and_methods()
    {
        var type = typeof(Request);

        var properties = new[]
        {
            "app", "baseUrl", "body", "cookies", "fresh", "host", "hostname", "ip", "ips",
            "method", "originalUrl", "params", "path", "protocol", "query", "res", "route",
            "secure", "signedCookies", "stale", "subdomains", "xhr"
        };
        foreach (var property in properties)
            assertProperty(type, property);

        var methods = new[]
        {
            "accepts", "acceptsCharsets", "acceptsEncodings", "acceptsLanguages",
            "get", "header", "is", "range"
        };
        foreach (var method in methods)
            assertMethod(type, method);
    }

    [Fact]
    public void response_exposes_expected_properties_and_methods()
    {
        var type = typeof(Response);

        var properties = new[] { "app", "headersSent", "locals", "req" };
        foreach (var property in properties)
            assertProperty(type, property);

        var methods = new[]
        {
            "append", "attachment", "cookie", "clearCookie", "download", "end",
            "format", "get", "json", "jsonp", "links", "location", "redirect",
            "render", "send", "sendFile", "sendStatus", "set", "status", "type", "vary"
        };
        foreach (var method in methods)
            assertMethod(type, method);
    }

    [Fact]
    public void router_exposes_expected_methods()
    {
        var type = typeof(Router);

        var methods = new[]
        {
            "all", "method", "param", "route", "use", "delete", "get", "post",
            "put", "checkout", "copy", "head", "lock_", "merge", "mkactivity",
            "mkcol", "move", "m_search", "notify", "options", "patch", "purge",
            "report", "search", "subscribe", "trace", "unlock", "unsubscribe"
        };
        foreach (var method in methods)
            assertMethod(type, method);
    }

    private static void assertMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type type, string methodName)
    {
        var found = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Any(method => string.Equals(method.Name, methodName, StringComparison.Ordinal));
        Assert.True(found, $"{type.Name} is missing method '{methodName}'");
    }

    private static void assertProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type type, string propertyName)
    {
        var found = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Any(property => string.Equals(property.Name, propertyName, StringComparison.Ordinal));
        Assert.True(found, $"{type.Name} is missing property '{propertyName}'");
    }

    private static void assertEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] Type type, string eventName)
    {
        var found = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Any(@event => string.Equals(@event.Name, eventName, StringComparison.Ordinal));
        Assert.True(found, $"{type.Name} is missing event '{eventName}'");
    }
}
