using System;
using System.Threading;

namespace express;

internal static class ProcessKeepAlive
{
    private static readonly object Gate = new();
    private static readonly ManualResetEventSlim Idle = new(initialState: true);

    private static int _activeServers;
    private static Thread? _thread;

    public static IDisposable Acquire()
    {
        lock (Gate)
        {
            _activeServers++;
            if (_activeServers == 1)
            {
                Idle.Reset();
                _thread = new Thread(() => Idle.Wait())
                {
                    IsBackground = false,
                    Name = "express.keepalive"
                };
                _thread.Start();
            }
        }

        return new ReleaseToken();
    }

    internal static int ActiveServerCount
    {
        get
        {
            lock (Gate)
            {
                return _activeServers;
            }
        }
    }

    internal static Thread? KeepAliveThread
    {
        get
        {
            lock (Gate)
            {
                return _thread;
            }
        }
    }

    private static void Release()
    {
        lock (Gate)
        {
            if (_activeServers <= 0)
                return;

            _activeServers--;
            if (_activeServers == 0)
            {
                Idle.Set();
                _thread = null;
            }
        }
    }

    private sealed class ReleaseToken : IDisposable
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
                return;

            Release();
        }
    }
}

