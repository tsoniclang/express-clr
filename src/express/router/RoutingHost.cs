using System;
using System.Threading.Tasks;

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
    public TSelf all(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, RequestHandler callback, params RequestHandler[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => all((object)path, callback, callbacks);
    public TSelf all(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => all((object)path, callback, callbacks);

    public TSelf checkout(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, RequestHandler callback, params RequestHandler[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => checkout((object)path, callback, callbacks);
    public TSelf checkout(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => checkout((object)path, callback, callbacks);

    public TSelf copy(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, RequestHandler callback, params RequestHandler[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => copy((object)path, callback, callbacks);
    public TSelf copy(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => copy((object)path, callback, callbacks);

    public TSelf delete(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, RequestHandler callback, params RequestHandler[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => delete((object)path, callback, callbacks);
    public TSelf delete(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => delete((object)path, callback, callbacks);

    public TSelf get(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, RequestHandler callback, params RequestHandler[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => get((object)path, callback, callbacks);
    public TSelf get(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => get((object)path, callback, callbacks);

    public TSelf head(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, RequestHandler callback, params RequestHandler[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => head((object)path, callback, callbacks);
    public TSelf head(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => head((object)path, callback, callbacks);

    public TSelf lock_(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, RequestHandler callback, params RequestHandler[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => lock_((object)path, callback, callbacks);
    public TSelf lock_(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => lock_((object)path, callback, callbacks);

    public TSelf merge(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, RequestHandler callback, params RequestHandler[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => merge((object)path, callback, callbacks);
    public TSelf merge(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => merge((object)path, callback, callbacks);

    public TSelf mkactivity(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, RequestHandler callback, params RequestHandler[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => mkactivity((object)path, callback, callbacks);
    public TSelf mkactivity(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => mkactivity((object)path, callback, callbacks);

    public TSelf mkcol(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, RequestHandler callback, params RequestHandler[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => mkcol((object)path, callback, callbacks);
    public TSelf mkcol(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => mkcol((object)path, callback, callbacks);

    public TSelf move(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, RequestHandler callback, params RequestHandler[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => move((object)path, callback, callbacks);
    public TSelf move(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => move((object)path, callback, callbacks);

    public TSelf m_search(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, RequestHandler callback, params RequestHandler[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => m_search((object)path, callback, callbacks);
    public TSelf m_search(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => m_search((object)path, callback, callbacks);

    public TSelf notify(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, RequestHandler callback, params RequestHandler[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => notify((object)path, callback, callbacks);
    public TSelf notify(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => notify((object)path, callback, callbacks);

    public TSelf options(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, RequestHandler callback, params RequestHandler[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => options((object)path, callback, callbacks);
    public TSelf options(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => options((object)path, callback, callbacks);

    public TSelf patch(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, RequestHandler callback, params RequestHandler[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => patch((object)path, callback, callbacks);
    public TSelf patch(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => patch((object)path, callback, callbacks);

    public TSelf post(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, RequestHandler callback, params RequestHandler[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => post((object)path, callback, callbacks);
    public TSelf post(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => post((object)path, callback, callbacks);

    public TSelf purge(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, RequestHandler callback, params RequestHandler[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => purge((object)path, callback, callbacks);
    public TSelf purge(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => purge((object)path, callback, callbacks);

    public TSelf put(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, RequestHandler callback, params RequestHandler[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => put((object)path, callback, callbacks);
    public TSelf put(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => put((object)path, callback, callbacks);

    public TSelf report(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, RequestHandler callback, params RequestHandler[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => report((object)path, callback, callbacks);
    public TSelf report(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => report((object)path, callback, callbacks);

    public TSelf search(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, RequestHandler callback, params RequestHandler[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => search((object)path, callback, callbacks);
    public TSelf search(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => search((object)path, callback, callbacks);

    public TSelf subscribe(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, RequestHandler callback, params RequestHandler[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => subscribe((object)path, callback, callbacks);
    public TSelf subscribe(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => subscribe((object)path, callback, callbacks);

    public TSelf trace(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, RequestHandler callback, params RequestHandler[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => trace((object)path, callback, callbacks);
    public TSelf trace(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => trace((object)path, callback, callbacks);

    public TSelf unlock(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, RequestHandler callback, params RequestHandler[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => unlock((object)path, callback, callbacks);
    public TSelf unlock(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => unlock((object)path, callback, callbacks);

    public TSelf unsubscribe(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, RequestHandler callback, params RequestHandler[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => unsubscribe((object)path, callback, callbacks);
    public TSelf unsubscribe(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => unsubscribe((object)path, callback, callbacks);

    public TSelf use(Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(RequestHandler callback, params RequestHandler[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(Func<Exception, Request, Response, NextFunction, Task> callback, params Func<Exception, Request, Response, NextFunction, Task>[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(Func<Exception, Request, Response, NextFunction, object?> callback, params Func<Exception, Request, Response, NextFunction, object?>[] callbacks) => use((object)"/", callback, callbacks);
    public TSelf use(Action<Exception, Request, Response, NextFunction> callback, params Action<Exception, Request, Response, NextFunction>[] callbacks) => use((object)"/", callback, callbacks);

    public TSelf use(string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, RequestHandler callback, params RequestHandler[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, Func<Exception, Request, Response, NextFunction, Task> callback, params Func<Exception, Request, Response, NextFunction, Task>[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, Func<Exception, Request, Response, NextFunction, object?> callback, params Func<Exception, Request, Response, NextFunction, object?>[] callbacks) => use((object)path, callback, callbacks);
    public TSelf use(string path, Action<Exception, Request, Response, NextFunction> callback, params Action<Exception, Request, Response, NextFunction>[] callbacks) => use((object)path, callback, callbacks);

    public Route route(string path) => route((object)path);

    public TSelf method(string method, string path, Func<Request, Response, Task> callback, params Func<Request, Response, Task>[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, Func<Request, Response, object?> callback, params Func<Request, Response, object?>[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, Action<Request, Response> callback, params Action<Request, Response>[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, RequestHandler callback, params RequestHandler[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, Func<Request, Response, NextFunction, object?> callback, params Func<Request, Response, NextFunction, object?>[] callbacks) => self.method(method, (object)path, callback, callbacks);
    public TSelf method(string method, string path, Action<Request, Response, NextFunction> callback, params Action<Request, Response, NextFunction>[] callbacks) => self.method(method, (object)path, callback, callbacks);

    public virtual TSelf all(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf checkout(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf copy(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf delete(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf get(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf head(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf lock_(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf merge(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf method(string method, object path, object callback, params object[] callbacks) => self;
    public virtual TSelf mkactivity(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf mkcol(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf move(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf m_search(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf notify(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf options(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf patch(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf post(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf purge(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf put(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf report(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf search(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf subscribe(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf trace(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf unlock(object path, object callback, params object[] callbacks) => self;
    public virtual TSelf unsubscribe(object path, object callback, params object[] callbacks) => self;

    public virtual TSelf param(string name, ParamHandler callback) => self;
    public virtual Route route(object path) => throw new NotSupportedException();
    public virtual TSelf use(object callback, params object[] callbacks) => self;
    public virtual TSelf use(object path, object callback, params object[] callbacks) => self;
}
