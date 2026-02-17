using express;
using Xunit;

namespace express.Tests.runtime;

public class params_runtime_tests
{
    [Fact]
    public void indexer_is_safe_and_coerces_values_to_string()
    {
        var p = new Params();
        Assert.Null(p["missing"]);

        p.Set("id", "42");
        Assert.Equal("42", p["id"]);

        // Case-insensitive keys (matches Express conventions).
        Assert.Equal("42", p["ID"]);

        p.Set("n", 123);
        Assert.Equal("123", p["n"]);
    }
}

