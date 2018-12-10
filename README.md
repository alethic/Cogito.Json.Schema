# Cogito.Json.Schema
Various utilities for working with JSON schema. Contains an incredibly fast JSchema validator.

```
var schema = new JSchema();
var validator = JSchemaExpressionBuilder.CreateDefault().Build(s).Compile();
validator.Invoke(new JObject());
```
