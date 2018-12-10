# Cogito.Json.Schema
Various utilities for working with JSON schema. Contains an incredibly fast JSchema validator.

## Cogito.Json.Schema.Validation

It's pretty simple to use. Take an existing Newtonsoft `JSchema`, loaded however you want, and use a `JSchemaExpressionBuilder` instance to generate the `Expression` tree. Then compile that. And you can invoke the result to return `true` or `false` to validate.

Obviously using the same compiled result repeatidly is recommended.

`JSchemaExpressionBuilder` is not thread safe. It caches some stuff internally to deal with recursive schemas. Have to lift a bunch of stuff out of that class to get that working better.

```
var schema = new JSchema();
var validator = JSchemaExpressionBuilder.CreateDefault().Build(schema).Compile();
validator.Invoke(new JObject());
```
