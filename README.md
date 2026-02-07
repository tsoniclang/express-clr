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

## Coverage Gate

```bash
npm run test:coverage
```

The coverage gate enforces `line`, `branch`, and `method` coverage at `100%` for the `expressjs` assembly.

## Compatibility Notes

See `docs/deviations.md` for known Express compatibility deviations.
See `docs/test-matrix.md` for API-level coverage mapping.

## License

MIT
