# expressjs-clr

`expressjs-clr` is the runtime implementation for Express-style APIs on ASP.NET Core.

It is the source of truth for behavior and parity decisions. The `expressjs` repo publishes the generated TypeScript package that targets this runtime.

## Scope

- Express 5.x-oriented API surface.
- ASP.NET Core primitives for server/request/response internals.
- Pragmatic parity: maximize behavioral compatibility while documenting remaining deviations.

## Quick Start

```csharp
using expressjs;

var app = express.create();

app.get("/", (Request _, Response res) =>
{
    res.send("hello");
});

app.listen(3000);
```

## Build

```bash
dotnet build src/expressjs/expressjs.csproj -c Release
```

## Test

```bash
dotnet test tests/expressjs.Tests/expressjs.Tests.csproj -c Release
```

## Coverage Gate

```bash
npm run test:coverage
```

The coverage gate enforces `line`, `branch`, and `method` coverage at `100%` for the `expressjs` assembly.

## Documentation Map

- Runtime architecture: `docs/architecture.md`
- Known compatibility deviations: `docs/deviations.md`
- API test/coverage matrix: `docs/test-matrix.md`

## Naming Notes

Some HTTP verbs require C#-safe identifiers:

- `lock_()` maps to `LOCK`
- `m_search()` maps to `M-SEARCH`

For exact-string verbs, use `method("...")`.

## License

MIT
