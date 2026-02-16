using System;
using System.Threading;
using Xunit;

namespace express.Tests.runtime;

public class process_keepalive_listen_tests
{
    [Fact]
    public void listen_starts_foreground_keepalive_thread_and_close_stops_it()
    {
        Assert.Equal(0, ProcessKeepAlive.ActiveServerCount);
        Assert.Null(ProcessKeepAlive.KeepAliveThread);

        var app = express.create();
        var port = test_runtime_utils.reserveTcpPort();
        var server = app.listen(port);

        var thread = ProcessKeepAlive.KeepAliveThread;
        Assert.NotNull(thread);
        Assert.False(thread!.IsBackground);
        Assert.True(thread.IsAlive);
        Assert.Equal(1, ProcessKeepAlive.ActiveServerCount);

        server.close();

        var stopped = SpinWait.SpinUntil(() => !thread.IsAlive, TimeSpan.FromSeconds(5));
        Assert.True(stopped);
        Assert.Equal(0, ProcessKeepAlive.ActiveServerCount);
        Assert.Null(ProcessKeepAlive.KeepAliveThread);
    }
}
