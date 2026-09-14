# Cogito.Json.Schema

[![Build](https://github.com/alethic/Cogito.Json.Schema/actions/workflows/Cogito.Json.Schema.yml/badge.svg)](https://github.com/alethic/Cogito.Json.Schema/actions/workflows/Cogito.Json.Schema.yml)

Compiles a JSON schema into a validation delegate, and provides tools for comparing and rewriting schemas.

## Packages

**[Cogito.Json.Schema](https://www.nuget.org/packages/Cogito.Json.Schema)** — Utilities for working with `JSchema` documents themselves — comparing them, cloning them, and rewriting them.

**[Cogito.Json.Schema.Validation](https://www.nuget.org/packages/Cogito.Json.Schema.Validation)** — Compiles a JSON schema into a validation delegate, for validating many documents against one schema.

Each package carries its own README with the detail; the links above go to nuget.org.

## Building

```shell
dotnet restore Cogito.Json.Schema.slnx
dotnet msbuild -p:Configuration=Release Cogito.Json.Schema.dist.msbuildproj
```

Packages are staged into `dist/nuget` and test suites into `dist/tests`; run a suite with
`dotnet test -f <tfm> <path to its assembly>`.

## License

MIT — see [LICENSE](LICENSE).
