# Cogito.Json.Schema.Validation

Compiles a JSON schema into a validation delegate, for validating many documents against one schema.

## Why

Newtonsoft's validator interprets the schema on every document. When the schema is fixed and the
documents are many — an ingest endpoint, a message consumer, a batch import — that interpretation is
repeated work. Compiling the schema to an expression tree once turns validation into straight-line
code, and it runs considerably faster as a result.

## Install

```shell
dotnet add package Cogito.Json.Schema.Validation
```

## Use

```csharp
var builder = new JSchemaExpressionBuilder();
var validate = builder.Build(schema).Compile();

if (validate(document) == false)
    Reject(document);
```

Build once, keep the delegate, call it per document.

Supports the draft versions in `SchemaVersions`, including the `format` and `contentEncoding`
vocabularies; `FormatExpressionBuilder` is the seam for adding a format of your own. Individual
keyword builders (`PropertyExpressionBuilder`, `OneOfExpressionBuilder`, and the rest) can be
replaced through `IExpressionBuilder`.

Built on [Newtonsoft.Json.Schema](https://www.newtonsoft.com/jsonschema), which is commercially
licensed — its licence terms apply to your use of it.

## License

MIT.
