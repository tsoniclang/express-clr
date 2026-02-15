namespace express;

public class Route : RoutingHost<Route>
{
    private readonly Router _router;
    public object path { get; }

    internal Route(Router router, object path)
    {
        _router = router;
        this.path = path;
    }

    public Route all(RouteHandler callback, params RouteHandler[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    public Route all(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    public Route all(RouteHandlerSync callback, params RouteHandlerSync[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    public Route all(RequestHandler callback, params RequestHandler[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    public Route all(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    public Route all(RequestHandlerSync callback, params RequestHandlerSync[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    internal Route all(object callback, params object[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    internal override Route all(object routePath, object callback, params object[] callbacks)
    {
        _router.all(path, callback, callbacks);
        return this;
    }

    internal override Route checkout(object routePath, object callback, params object[] callbacks)
    {
        _router.checkout(path, callback, callbacks);
        return this;
    }

    internal override Route copy(object routePath, object callback, params object[] callbacks)
    {
        _router.copy(path, callback, callbacks);
        return this;
    }

    internal Route delete(object callback, params object[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public Route delete(RouteHandler callback, params RouteHandler[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public Route delete(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public Route delete(RouteHandlerSync callback, params RouteHandlerSync[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public Route delete(RequestHandler callback, params RequestHandler[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public Route delete(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    public Route delete(RequestHandlerSync callback, params RequestHandlerSync[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    internal override Route delete(object routePath, object callback, params object[] callbacks)
    {
        _router.delete(path, callback, callbacks);
        return this;
    }

    internal Route get(object callback, params object[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public Route get(RouteHandler callback, params RouteHandler[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public Route get(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public Route get(RouteHandlerSync callback, params RouteHandlerSync[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public Route get(RequestHandler callback, params RequestHandler[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public Route get(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    public Route get(RequestHandlerSync callback, params RequestHandlerSync[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    internal override Route get(object routePath, object callback, params object[] callbacks)
    {
        _router.get(path, callback, callbacks);
        return this;
    }

    internal override Route head(object routePath, object callback, params object[] callbacks)
    {
        _router.head(path, callback, callbacks);
        return this;
    }

    internal override Route lock_(object routePath, object callback, params object[] callbacks)
    {
        _router.lock_(path, callback, callbacks);
        return this;
    }

    internal override Route merge(object routePath, object callback, params object[] callbacks)
    {
        _router.merge(path, callback, callbacks);
        return this;
    }

    internal override Route method(string method, object routePath, object callback, params object[] callbacks)
    {
        _router.method(method, path, callback, callbacks);
        return this;
    }

    internal override Route mkactivity(object routePath, object callback, params object[] callbacks)
    {
        _router.mkactivity(path, callback, callbacks);
        return this;
    }

    internal override Route mkcol(object routePath, object callback, params object[] callbacks)
    {
        _router.mkcol(path, callback, callbacks);
        return this;
    }

    internal override Route move(object routePath, object callback, params object[] callbacks)
    {
        _router.move(path, callback, callbacks);
        return this;
    }

    internal override Route m_search(object routePath, object callback, params object[] callbacks)
    {
        _router.m_search(path, callback, callbacks);
        return this;
    }

    internal override Route notify(object routePath, object callback, params object[] callbacks)
    {
        _router.notify(path, callback, callbacks);
        return this;
    }

    internal Route options(object callback, params object[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public Route options(RouteHandler callback, params RouteHandler[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public Route options(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public Route options(RouteHandlerSync callback, params RouteHandlerSync[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public Route options(RequestHandler callback, params RequestHandler[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public Route options(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    public Route options(RequestHandlerSync callback, params RequestHandlerSync[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    internal override Route options(object routePath, object callback, params object[] callbacks)
    {
        _router.options(path, callback, callbacks);
        return this;
    }

    internal Route patch(object callback, params object[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public Route patch(RouteHandler callback, params RouteHandler[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public Route patch(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public Route patch(RouteHandlerSync callback, params RouteHandlerSync[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public Route patch(RequestHandler callback, params RequestHandler[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public Route patch(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    public Route patch(RequestHandlerSync callback, params RequestHandlerSync[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    internal override Route patch(object routePath, object callback, params object[] callbacks)
    {
        _router.patch(path, callback, callbacks);
        return this;
    }

    internal Route post(object callback, params object[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public Route post(RouteHandler callback, params RouteHandler[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public Route post(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public Route post(RouteHandlerSync callback, params RouteHandlerSync[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public Route post(RequestHandler callback, params RequestHandler[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public Route post(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    public Route post(RequestHandlerSync callback, params RequestHandlerSync[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    internal override Route post(object routePath, object callback, params object[] callbacks)
    {
        _router.post(path, callback, callbacks);
        return this;
    }

    internal override Route purge(object routePath, object callback, params object[] callbacks)
    {
        _router.purge(path, callback, callbacks);
        return this;
    }

    internal Route put(object callback, params object[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public Route put(RouteHandler callback, params RouteHandler[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public Route put(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public Route put(RouteHandlerSync callback, params RouteHandlerSync[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public Route put(RequestHandler callback, params RequestHandler[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public Route put(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    public Route put(RequestHandlerSync callback, params RequestHandlerSync[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    internal override Route put(object routePath, object callback, params object[] callbacks)
    {
        _router.put(path, callback, callbacks);
        return this;
    }

    internal override Route report(object routePath, object callback, params object[] callbacks)
    {
        _router.report(path, callback, callbacks);
        return this;
    }

    internal override Route search(object routePath, object callback, params object[] callbacks)
    {
        _router.search(path, callback, callbacks);
        return this;
    }

    internal override Route subscribe(object routePath, object callback, params object[] callbacks)
    {
        _router.subscribe(path, callback, callbacks);
        return this;
    }

    internal override Route trace(object routePath, object callback, params object[] callbacks)
    {
        _router.trace(path, callback, callbacks);
        return this;
    }

    internal override Route unlock(object routePath, object callback, params object[] callbacks)
    {
        _router.unlock(path, callback, callbacks);
        return this;
    }

    internal override Route unsubscribe(object routePath, object callback, params object[] callbacks)
    {
        _router.unsubscribe(path, callback, callbacks);
        return this;
    }
}
