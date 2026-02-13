using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace express;

public delegate Task NextFunction(string? control);
public delegate Task RequestHandler(Request req, Response res, NextFunction next);
public delegate void RequestHandlerSync(Request req, Response res, NextFunction next);
public delegate object? RequestHandlerReturn(Request req, Response res, NextFunction next);
public delegate Task RouteHandler(Request req, Response res);
public delegate void RouteHandlerSync(Request req, Response res);
public delegate object? RouteHandlerReturn(Request req, Response res);
public delegate Task ErrorRequestHandler(Exception err, Request req, Response res, NextFunction next);
public delegate void ErrorRequestHandlerSync(Exception err, Request req, Response res, NextFunction next);
public delegate object? ErrorRequestHandlerReturn(Exception err, Request req, Response res, NextFunction next);
public delegate Task ParamHandler(Request req, Response res, NextFunction next, object? value, string name);
public delegate void VerifyBodyHandler(Request req, Response res, byte[] buffer, string? encoding);
public delegate bool MediaTypeMatcher(Request req);
public delegate string CookieEncoder(string value);
public delegate object? QueryParser(string queryString);
public delegate bool TrustProxyEvaluator(string ip);
public delegate void TemplateEngine(string path, Dictionary<string, object?> options, Action<Exception?, string?> callback);
public delegate void SetHeadersHandler(Response res, string path, FileStat stat);
