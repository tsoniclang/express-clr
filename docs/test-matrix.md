# Express 5.x API Test Matrix

This matrix tracks runtime coverage for the current `expressjs-clr` API surface.

## Coverage Gate

- Command: `npm run test:coverage`
- Thresholds: `line=100`, `branch=100`, `method=100`
- Current result: `100% / 100% / 100%` on `expressjs` module

## Matrix

| Area | API surface | Coverage mode | Status |
|---|---|---|---|
| `express` module | `create`, `application`, `app`, `json`, `raw`, `Router`, `static`, `text`, `urlencoded` | End-to-end middleware routing tests + option branch tests | Covered |
| `Application` properties/events | `locals`, `mountpath`, `router`, `mount` | Runtime tests with mounted sub-apps | Covered |
| `Application` methods | `all`, full HTTP verb set, `method`, `disable/disabled`, `enable/enabled`, `engine`, `get`, `listen` overloads, `param`, `path`, `render` overloads, `route`, `set`, `use` overloads | End-to-end requests + lifecycle tests | Covered |
| `Request` properties | `app`, `baseUrl`, `body`, `cookies`, `fresh`, `host`, `hostname`, `ip`, `ips`, `method`, `originalUrl`, `params`, `path`, `protocol`, `query`, `res`, `route`, `secure`, `signedCookies`, `stale`, `subdomains`, `xhr`, `signed` | Constructor and runtime behavior tests | Covered |
| `Request` methods | `accepts`, `acceptsCharsets`, `acceptsEncodings`, `acceptsLanguages`, `get`, `header`, `is`, `range`, `setHeader` | Input/edge-case branch tests | Covered |
| `Response` properties | `app`, `headersSent`, `locals`, `req` | Runtime tests with and without `HttpContext` | Covered |
| `Response` methods | `append`, `attachment`, `cookie`, `clearCookie`, `download`, `end`, `format`, `get`, `json`, `jsonp`, `links`, `location`, `redirect`, `render` overloads, `send`, `sendFile`, `sendStatus`, `set`, `header`, `status`, `type`, `contentType`, `vary` | End-to-end response and file-serving tests | Covered |
| `Router` methods | `all`, full HTTP verb set, `method`, `param`, `route`, `use` overloads | End-to-end route matching and control-flow tests | Covered |
| `Route` methods | `all`, full HTTP verb set, `method`, chaining helpers (`get/post/put/delete/options/patch`) | End-to-end chaining and override-path tests | Covered |
| Infrastructure types | `AppServer`, option/config classes, `RoutingHost<TSelf>`, path/export internals | Branch and behavior tests | Covered |

## Test Files

- `tests/expressjs.Tests/runtime/coverage.matrix.application.tests.cs`
- `tests/expressjs.Tests/runtime/coverage.matrix.express.tests.cs`
- `tests/expressjs.Tests/runtime/coverage.matrix.infrastructure.tests.cs`
- `tests/expressjs.Tests/runtime/coverage.matrix.request.tests.cs`
- `tests/expressjs.Tests/runtime/coverage.matrix.response.tests.cs`
- `tests/expressjs.Tests/runtime/coverage.matrix.routing.tests.cs`
- Existing runtime and surface suites remain in place as regression tests.
