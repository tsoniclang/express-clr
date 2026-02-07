namespace expressjs;

public class Route : RoutingHost<Route>
{
    private readonly Router _router;
    public object path { get; }

    internal Route(Router router, object path)
    {
        _router = router;
        this.path = path;
    }

    public Route all(object callback, params object[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    public override Route all(object routePath, object callback, params object[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    public override Route checkout(object routePath, object callback, params object[] callbacks)
    {
        _router.checkout(path, callback, callbacks);
        return this;
    }

    public override Route copy(object routePath, object callback, params object[] callbacks)
    {
        _router.copy(path, callback, callbacks);
        return this;
    }

    public Route delete(object callback, params object[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public override Route delete(object routePath, object callback, params object[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public Route get(object callback, params object[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public override Route get(object routePath, object callback, params object[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public override Route head(object routePath, object callback, params object[] callbacks)
    {
        _router.head(path, callback, callbacks);
        return this;
    }

    public override Route lock_(object routePath, object callback, params object[] callbacks)
    {
        _router.lock_(path, callback, callbacks);
        return this;
    }

    public override Route merge(object routePath, object callback, params object[] callbacks)
    {
        _router.merge(path, callback, callbacks);
        return this;
    }

    public override Route method(string method, object routePath, object callback, params object[] callbacks)
    {
        _router.method(method, path, callback, callbacks);
        return this;
    }

    public override Route mkactivity(object routePath, object callback, params object[] callbacks)
    {
        _router.mkactivity(path, callback, callbacks);
        return this;
    }

    public override Route mkcol(object routePath, object callback, params object[] callbacks)
    {
        _router.mkcol(path, callback, callbacks);
        return this;
    }

    public override Route move(object routePath, object callback, params object[] callbacks)
    {
        _router.move(path, callback, callbacks);
        return this;
    }

    public override Route m_search(object routePath, object callback, params object[] callbacks)
    {
        _router.m_search(path, callback, callbacks);
        return this;
    }

    public override Route notify(object routePath, object callback, params object[] callbacks)
    {
        _router.notify(path, callback, callbacks);
        return this;
    }

    public Route options(object callback, params object[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public override Route options(object routePath, object callback, params object[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public Route patch(object callback, params object[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public override Route patch(object routePath, object callback, params object[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public Route post(object callback, params object[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public override Route post(object routePath, object callback, params object[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public override Route purge(object routePath, object callback, params object[] callbacks)
    {
        _router.purge(path, callback, callbacks);
        return this;
    }

    public Route put(object callback, params object[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public override Route put(object routePath, object callback, params object[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public override Route report(object routePath, object callback, params object[] callbacks)
    {
        _router.report(path, callback, callbacks);
        return this;
    }

    public override Route search(object routePath, object callback, params object[] callbacks)
    {
        _router.search(path, callback, callbacks);
        return this;
    }

    public override Route subscribe(object routePath, object callback, params object[] callbacks)
    {
        _router.subscribe(path, callback, callbacks);
        return this;
    }

    public override Route trace(object routePath, object callback, params object[] callbacks)
    {
        _router.trace(path, callback, callbacks);
        return this;
    }

    public override Route unlock(object routePath, object callback, params object[] callbacks)
    {
        _router.unlock(path, callback, callbacks);
        return this;
    }

    public override Route unsubscribe(object routePath, object callback, params object[] callbacks)
    {
        _router.unsubscribe(path, callback, callbacks);
        return this;
    }
}
