using System;

namespace express;

public abstract class RoutingHost<TSelf> where TSelf : RoutingHost<TSelf>
{
    protected TSelf self => (TSelf)this;

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
