using System;
using System.Security.Cryptography;
using System.Text;

namespace express;

internal static class CookieSignature
{
    public static string sign(string value, string secret)
    {
        var sig = signature(value, secret);
        return $"s:{value}.{sig}";
    }

    public static string? unsign(string signedValue, string secret)
    {
        if (!signedValue.StartsWith("s:", StringComparison.Ordinal))
            return null;

        var raw = signedValue[2..];
        var dot = raw.LastIndexOf('.');
        if (dot <= 0 || dot == raw.Length - 1)
            return null;

        var value = raw[..dot];
        var sig = raw[(dot + 1)..];
        var expected = signature(value, secret);
        if (!fixedTimeEquals(sig, expected))
            return null;

        return value;
    }

    private static string signature(string value, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
        return Convert.ToBase64String(hash).TrimEnd('=');
    }

    private static bool fixedTimeEquals(string a, string b)
    {
        var bytesA = Encoding.UTF8.GetBytes(a);
        var bytesB = Encoding.UTF8.GetBytes(b);
        if (bytesA.Length != bytesB.Length)
            return false;
        return CryptographicOperations.FixedTimeEquals(bytesA, bytesB);
    }
}

