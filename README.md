# expressjs-clr

`expressjs-clr` is a .NET library that provides an Express-style API surface for Tsonic projects.

It is designed for API parity and type-safe interop, not as a byte-for-byte runtime clone of Node.js Express internals.

## Build

```bash
dotnet build src/expressjs/expressjs.csproj -c Release
```

## Test

```bash
dotnet test tests/expressjs.Tests/expressjs.Tests.csproj -c Release
```

## Compatibility Notes

See `docs/deviations.md` for known Express compatibility deviations.

## License

MIT
