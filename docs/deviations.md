# Express 5.x Compatibility Deviations

`expressjs-clr` is implemented on ASP.NET Core primitives and targets maximum practical Express API coverage.

Known deviations:

1. Top-level callable `express()` is represented via static factory methods (`express.create()`, `express.app()`).
2. Some verb names are adapted for C# identifiers (`lock_`, `m_search`); `method("...")` remains available for exact-verb routing.
3. `next('router')` behavior is best-effort in the current flattened mount model.
4. Advanced path pattern behavior is best-effort; common string and `:param` patterns are covered.
5. Middleware internals for `json`, `raw`, `text`, `urlencoded`, and `static` are close but not fully byte-for-byte with upstream Node middleware stacks.
6. Cookie signing/signed cookie behavior is similar but not fully equivalent to `cookie-parser` edge cases.
7. Delegate handler dispatch is reflection-free; unsupported delegate signatures are ignored instead of reflection-invoked.
8. JSON body/object serialization is reflection-free. Fully supported shapes are primitives, `Dictionary<string, object?>`, arrays/lists, and `JsonElement`/`JsonDocument`. Arbitrary CLR objects (including anonymous objects) are not reflection-serialized.

The test suite tracks API surface and core runtime semantics. Deviations are expected to shrink over time.
