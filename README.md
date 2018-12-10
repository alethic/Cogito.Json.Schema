# Cogito.Json.Schema
Various utilities for working with JSON schema. Contains an incredibly fast JSchema validator.

It's pretty simple to use. Take an existing Newtonsoft `JSchema`, loaded however you want, and use a `JSchemaExpressionBuilder` instance to generate the `Expression` tree. Then compile that. And you can invoke the result to return `true` or `false` to validate.

Obviously using the same compiled result repeatidly is recommended.

```
var schema = new JSchema();
var validator = JSchemaExpressionBuilder.CreateDefault().Build(schema).Compile();
validator.Invoke(new JObject());
```
