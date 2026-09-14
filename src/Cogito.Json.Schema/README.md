# Cogito.Json.Schema

Utilities for working with `JSchema` documents themselves — comparing them, cloning them, and
rewriting them.

## Why

Schemas get treated as opaque once loaded, so questions like "are these two schemas the same?" or
"give me this schema with every `$ref` resolved" end up answered with string comparison or by hand.

## Install

```shell
dotnet add package Cogito.Json.Schema
```

## Use

Compare two schemas structurally rather than textually:

```csharp
var same = new JSchemaEqualityComparer().Equals(a, b);
```

Rewrite a schema by deriving from `JSchemaTransformer`, or assemble a transformation from delegates
with `JSchemaDelegatingTransformer`:

```csharp
var flattened = new MyTransformer().Transform(schema);
```

`ToJObject` / `ToJToken` render a schema back to JSON, and `Clone` gives you a deep copy you can
modify safely.

For validation see `Cogito.Json.Schema.Validation`.

## License

MIT.
