# Runtime Architecture

This document describes how `expressjs-clr` maps Express-style APIs onto ASP.NET Core.

## Design Principles

- Prefer ASP.NET Core primitives over emulating Node internals.
- Preserve Express API shape where feasible.
- Keep deviations explicit and small.
- Validate behavior with end-to-end tests and strict coverage gates.

## Core Mapping

| Express concept | `expressjs-clr` type | ASP.NET Core primitive |
|---|---|---|
| Top-level factory (`express()`) | `express.create()` / `express.app()` | `WebApplication` creation in `Application.listen(...)` |
| App/router pipeline | `Application : Router` and `Router` layers | `HttpContext` dispatch loop |
| Request wrapper | `Request` | `HttpContext.Request` |
| Response wrapper | `Response` | `HttpContext.Response` |
| Route chain | `Route` | Router layer registration |
| Server handle | `AppServer` | Managed host lifecycle callbacks |

## Request/Response Flow

1. `Application.listen(...)` builds and starts a `WebApplication`.
2. Incoming `HttpContext` is converted into `Request`/`Response`.
3. Router layers are evaluated in registration order.
4. Matching handlers execute with Express-style `next(...)` control flow.
5. `Response` writes headers/body through `HttpContext.Response`.

## Routing Model

- Route and middleware registrations are stored as ordered layers.
- Supported path types include strings, arrays, and regex patterns.
- String path features prioritize common Express patterns:
  - Exact paths
  - Prefix middleware matching
  - `:param` extraction
  - `{*splat}` prefix wildcard semantics
- `param(...)` handlers run before route handlers and are deduplicated per request key/value pair.

## Mounting Model

- `app.use(path, routerOrApp)` exports mounted layers into the parent pipeline.
- `mount` events are emitted for mounted sub-apps.
- `next("router")` is supported best-effort in the flattened mount model.

## Middleware and Body Parsing

Built-in middleware surfaces:

- `express.json(...)`
- `express.raw(...)`
- `express.text(...)`
- `express.urlencoded(...)`
- `express.static(...)`

Behavior is implemented close to Express semantics, with remaining differences tracked in `docs/deviations.md`.

## Rendering

- Template engines are registered via `app.engine(ext, callback)`.
- `app.render(...)` and `res.render(...)` use registered engines when available.
- Without an engine, the runtime returns deterministic fallback rendered markers.

## File/Download Responses

- `res.sendFile(...)` and `res.download(...)` map to filesystem + `HttpContext.Response` file send behavior.
- Relative paths can be resolved via `root` options.
- Error callbacks follow Express-like signatures.

## Runtime vs Types Repo

- Runtime semantics belong in `expressjs-clr` docs.
- Generated TypeScript package and release workflow belong in `expressjs` docs.
