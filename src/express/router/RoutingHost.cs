using System;

namespace express;

public abstract class RoutingHost<TSelf> where TSelf : RoutingHost<TSelf>
{
    protected TSelf self => (TSelf)this;

    // Strongly-typed overloads for the common Express calling patterns.
    // These forward to the object-based API (which is what Router/Route override),
    // but let tsbindgen emit useful TS signatures (instead of unknown).
    //
    // Design note (airplane-grade):
    // - We intentionally only surface Task-returning handler delegates here.
    //   In TypeScript, Promise-returning functions are assignable to void-returning
    //   callback types, which can lead to "async-void" execution if sync overloads
    //   exist. Keeping the surface Task-only avoids that entire class of bugs.
    public TSelf all(string path, RequestHandler callback, params RequestHandler[] callbacks) => all((object)path, callback, callbacks);
    public TSelf checkout(string path, RequestHandler callback, params RequestHandler[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf copy(string path, RequestHandler callback, params RequestHandler[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf delete(string path, RequestHandler callback, params RequestHandler[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf get(string path, RequestHandler callback, params RequestHandler[] callbacks) => get((object)path, callback, callbacks);
    public TSelf head(string path, RequestHandler callback, params RequestHandler[] callbacks) => head((object)path, callback, callbacks);
    public TSelf lock_(string path, RequestHandler callback, params RequestHandler[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf merge(string path, RequestHandler callback, params RequestHandler[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf mkactivity(string path, RequestHandler callback, params RequestHandler[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkcol(string path, RequestHandler callback, params RequestHandler[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf move(string path, RequestHandler callback, params RequestHandler[] callbacks) => move((object)path, callback, callbacks);
    public TSelf m_search(string path, RequestHandler callback, params RequestHandler[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf notify(string path, RequestHandler callback, params RequestHandler[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf options(string path, RequestHandler callback, params RequestHandler[] callbacks) => options((object)path, callback, callbacks);
    public TSelf patch(string path, RequestHandler callback, params RequestHandler[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf post(string path, RequestHandler callback, params RequestHandler[] callbacks) => post((object)path, callback, callbacks);
    public TSelf purge(string path, RequestHandler callback, params RequestHandler[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf put(string path, RequestHandler callback, params RequestHandler[] callbacks) => put((object)path, callback, callbacks);
    public TSelf report(string path, RequestHandler callback, params RequestHandler[] callbacks) => report((object)path, callback, callbacks);
    public TSelf search(string path, RequestHandler callback, params RequestHandler[] callbacks) => search((object)path, callback, callbacks);
    public TSelf subscribe(string path, RequestHandler callback, params RequestHandler[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf trace(string path, RequestHandler callback, params RequestHandler[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf unlock(string path, RequestHandler callback, params RequestHandler[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, RequestHandler callback, params RequestHandler[] callbacks) => unsubscribe((object)path, callback, callbacks);

    public TSelf use(RequestHandler callback, params RequestHandler[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(string path, RequestHandler callback, params RequestHandler[] callbacks) => use((object)path, callback, callbacks);

    public TSelf useError(ErrorRequestHandler callback, params ErrorRequestHandler[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf useError(string path, ErrorRequestHandler callback, params ErrorRequestHandler[] callbacks) => use((object)path, callback, callbacks);

    public Route route(string path) => route((object)path);

    public TSelf method(string method, string path, RequestHandler callback, params RequestHandler[] callbacks) => self.method(method, (object)path, callback, callbacks);

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

