using System;
using Tsonic.JSRuntime;

namespace express;

internal static class js_interop
{
    public static int toInt32(string parameterName, double value)
    {
        validateFiniteInteger(parameterName, value);
        if (value < int.MinValue || value > int.MaxValue)
            throw new ArgumentOutOfRangeException(parameterName, $"Expected a 32-bit integer, got {value}.");
        return checked((int)value);
    }

    public static int? toNullableInt32(string parameterName, double? value)
    {
        return value is null ? null : toInt32(parameterName, value.Value);
    }

    public static long toInt64(string parameterName, double value)
    {
        validateFiniteInteger(parameterName, value);
        if (value < long.MinValue || value > long.MaxValue)
            throw new ArgumentOutOfRangeException(parameterName, $"Expected a 64-bit integer, got {value}.");
        return checked((long)value);
    }

    public static long? toNullableInt64(string parameterName, double? value)
    {
        return value is null ? null : toInt64(parameterName, value.Value);
    }

    public static double fromInt32(int value) => value;
    public static double? fromInt32(int? value) => value;
    public static double fromInt64(long value) => value;
    public static double? fromInt64(long? value) => value;

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

    private static void validateFiniteInteger(string parameterName, double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentOutOfRangeException(parameterName, $"Expected a finite number, got {value}.");
        if (System.Math.Truncate(value) != value)
            throw new ArgumentException($"Expected an integer value, got {value}.", parameterName);
    }
}
