using System;
using Tsonic.JSRuntime;

namespace express;

internal static class js_interop
{
    public static Date fromDateTime(DateTime value)
    {
        var utc = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        return new Date(new DateTimeOffset(utc).ToUnixTimeMilliseconds());
    }

    public static Date? fromDateTime(DateTime? value)
    {
        return value is null ? null : fromDateTime(value.Value);
    }

    public static Error? fromException(Exception? value)
    {
        return value is null ? null : value as Error ?? new Error(value.Message, value);
    }
}
