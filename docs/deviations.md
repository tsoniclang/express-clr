# Express 5.x Compatibility Deviations

`expressjs-clr` is implemented on ASP.NET Core primitives and targets maximum practical Express API coverage.

Known deviations:

1. Top-level callable `express()` is represented via static factory methods (`express.create()`, `express.app()`).
2. `next('router')` behavior is best-effort in the current flattened mount model.
3. Advanced path pattern behavior is best-effort; common string and `:param` patterns are covered.
4. Middleware internals for `json`, `raw`, `text`, `urlencoded`, and `static` are close but not fully byte-for-byte with upstream Node middleware stacks.
5. Cookie signing/signed cookie behavior is similar but not fully equivalent to `cookie-parser` edge cases.

The test suite tracks API surface and core runtime semantics. Deviations are expected to shrink over time.
