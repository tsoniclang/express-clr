using System;

namespace express;

public abstract class RoutingHost<TSelf> where TSelf : RoutingHost<TSelf>
{
    protected TSelf self => (TSelf)this;

    // Strongly-typed overloads for the common Express calling patterns.
    // These forward to the object-based API (which is what Router/Route override),
    // but let tsbindgen emit useful TS signatures (instead of unknown).
    //
    // Note: overload ordering matters in TypeScript; keep value-returning handlers
    // ahead of void-returning handlers to avoid accidental "void" selection.
    public TSelf all(string path, RouteHandler callback, params RouteHandler[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, RequestHandler callback, params RequestHandler[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => all((object)path, callback, callbacks);

    public TSelf checkout(string path, RouteHandler callback, params RouteHandler[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, RequestHandler callback, params RequestHandler[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => checkout((object)path, callback, callbacks);

    public TSelf copy(string path, RouteHandler callback, params RouteHandler[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, RequestHandler callback, params RequestHandler[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => copy((object)path, callback, callbacks);

    public TSelf delete(string path, RouteHandler callback, params RouteHandler[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, RequestHandler callback, params RequestHandler[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => delete((object)path, callback, callbacks);

    public TSelf get(string path, RouteHandler callback, params RouteHandler[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, RequestHandler callback, params RequestHandler[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => get((object)path, callback, callbacks);

    public TSelf head(string path, RouteHandler callback, params RouteHandler[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, RequestHandler callback, params RequestHandler[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => head((object)path, callback, callbacks);

    public TSelf lock_(string path, RouteHandler callback, params RouteHandler[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, RequestHandler callback, params RequestHandler[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => lock_((object)path, callback, callbacks);

    public TSelf merge(string path, RouteHandler callback, params RouteHandler[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, RequestHandler callback, params RequestHandler[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => merge((object)path, callback, callbacks);

    public TSelf mkactivity(string path, RouteHandler callback, params RouteHandler[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, RequestHandler callback, params RequestHandler[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => mkactivity((object)path, callback, callbacks);

    public TSelf mkcol(string path, RouteHandler callback, params RouteHandler[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, RequestHandler callback, params RequestHandler[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => mkcol((object)path, callback, callbacks);

    public TSelf move(string path, RouteHandler callback, params RouteHandler[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, RequestHandler callback, params RequestHandler[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => move((object)path, callback, callbacks);

    public TSelf m_search(string path, RouteHandler callback, params RouteHandler[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, RequestHandler callback, params RequestHandler[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => m_search((object)path, callback, callbacks);

    public TSelf notify(string path, RouteHandler callback, params RouteHandler[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, RequestHandler callback, params RequestHandler[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => notify((object)path, callback, callbacks);

    public TSelf options(string path, RouteHandler callback, params RouteHandler[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, RequestHandler callback, params RequestHandler[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => options((object)path, callback, callbacks);

    public TSelf patch(string path, RouteHandler callback, params RouteHandler[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, RequestHandler callback, params RequestHandler[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => patch((object)path, callback, callbacks);

    public TSelf post(string path, RouteHandler callback, params RouteHandler[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, RequestHandler callback, params RequestHandler[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => post((object)path, callback, callbacks);

    public TSelf purge(string path, RouteHandler callback, params RouteHandler[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, RequestHandler callback, params RequestHandler[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => purge((object)path, callback, callbacks);

    public TSelf put(string path, RouteHandler callback, params RouteHandler[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, RequestHandler callback, params RequestHandler[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => put((object)path, callback, callbacks);

    public TSelf report(string path, RouteHandler callback, params RouteHandler[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, RequestHandler callback, params RequestHandler[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => report((object)path, callback, callbacks);

    public TSelf search(string path, RouteHandler callback, params RouteHandler[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, RequestHandler callback, params RequestHandler[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => search((object)path, callback, callbacks);

    public TSelf subscribe(string path, RouteHandler callback, params RouteHandler[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, RequestHandler callback, params RequestHandler[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => subscribe((object)path, callback, callbacks);

    public TSelf trace(string path, RouteHandler callback, params RouteHandler[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, RequestHandler callback, params RequestHandler[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => trace((object)path, callback, callbacks);

    public TSelf unlock(string path, RouteHandler callback, params RouteHandler[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, RequestHandler callback, params RequestHandler[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => unlock((object)path, callback, callbacks);

    public TSelf unsubscribe(string path, RouteHandler callback, params RouteHandler[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, RequestHandler callback, params RequestHandler[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => unsubscribe((object)path, callback, callbacks);

    public TSelf use(RouteHandler callback, params RouteHandler[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(RequestHandler callback, params RequestHandler[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(ErrorRequestHandler callback, params ErrorRequestHandler[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(ErrorRequestHandlerReturn callback, params ErrorRequestHandlerReturn[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(ErrorRequestHandlerSync callback, params ErrorRequestHandlerSync[] callbacks) => use((object)"/", callback, callbacks);

    // Router/app mounting overloads (best-effort parity with Express.js).
    public TSelf use(Router router) => use((object)router);
    public TSelf use(string path, Router router) => use((object)path, router);

    public TSelf use(string path, RouteHandler callback, params RouteHandler[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, RequestHandler callback, params RequestHandler[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, ErrorRequestHandler callback, params ErrorRequestHandler[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, ErrorRequestHandlerReturn callback, params ErrorRequestHandlerReturn[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, ErrorRequestHandlerSync callback, params ErrorRequestHandlerSync[] callbacks) => use((object)path, callback, callbacks);

    public Route route(string path) => route((object)path);

    public TSelf method(string method, string path, RouteHandler callback, params RouteHandler[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, RouteHandlerReturn callback, params RouteHandlerReturn[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, RouteHandlerSync callback, params RouteHandlerSync[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, RequestHandler callback, params RequestHandler[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, RequestHandlerReturn callback, params RequestHandlerReturn[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, RequestHandlerSync callback, params RequestHandlerSync[] callbacks) => self.method(method, (object)path, callback, callbacks);

    // Object-based core API (internal): used by strongly-typed wrappers above and overridden by Router/Route.
    // We intentionally keep these out of the public surface so tsbindgen does not have to emit `unknown`
    // catchall overloads for JS consumers.
    internal virtual TSelf all(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf checkout(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf copy(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf delete(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf get(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf head(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf lock_(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf merge(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf method(string method, object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf mkactivity(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf mkcol(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf move(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf m_search(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf notify(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf options(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf patch(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf post(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf purge(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf put(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf report(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf search(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf subscribe(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf trace(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf unlock(object path, object callback, params object[] callbacks) => self;
    internal virtual TSelf unsubscribe(object path, object callback, params object[] callbacks) => self;

    public virtual TSelf param(string name, ParamHandler callback) => self;
    internal virtual Route route(object path) => throw new NotSupportedException();
    internal virtual TSelf use(object callback, params object[] callbacks) => self;
    internal virtual TSelf use(object path, object callback, params object[] callbacks) => self;
}
