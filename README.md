# express-clr

`express-clr` is the runtime implementation for Express-style APIs on ASP.NET Core.

It is the source of truth for behavior and parity decisions. The `express` repo publishes the generated TypeScript package that targets this runtime.

## Scope

- Express 5.x-oriented API surface.
- ASP.NET Core primitives for server/request/response internals.
- Pragmatic parity: maximize behavioral compatibility while documenting remaining deviations.

## Quick Start

```csharp
using express;

var app = express.create();

app.get("/", (Request _, Response res) =>
{
    res.send("hello");
});

app.listen(3000);
```

## Build

```bash
dotnet build src/express/express.csproj -c Release
```

## NativeAOT Validation

`express-clr` is designed for NativeAOT-first usage on ASP.NET Core primitives.

Validation command:

```bash
dotnet publish src/express/express.csproj -c Release -r linux-x64 -p:PublishAot=true -warnaserror
```

Current runtime avoids `DynamicInvoke` and reflection-based `System.Text.Json` serialization/deserialization paths.

## Test

```bash
dotnet test tests/express.Tests/express.Tests.csproj -c Release
```

## Coverage Gate

```bash
npm run test:coverage
```

The coverage gate enforces `line`, `branch`, and `method` coverage at `100%` for the `express` assembly.

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
