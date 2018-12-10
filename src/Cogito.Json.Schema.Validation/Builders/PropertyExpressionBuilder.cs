using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Validation.Builders
{

    public class PropertyExpressionBuilder : ExpressionBuilderBase
    {

        public override Expression Build(JSchemaExpressionBuilder builder, JSchema schema, Expression token)
        {
            return BuildProperties(builder, schema, token);
        }

        Expression BuildProperties(JSchemaExpressionBuilder builder, JSchema schema, Expression o)
        {
            return AllOf(BuildPropertiesAll(builder, schema, o).Where(i => i != null));
        }

        IEnumerable<Expression> BuildPropertiesAll(JSchemaExpressionBuilder builder, JSchema schema, Expression o)
        {
            if (schema.Properties.Count > 0)
                yield return IfThenElseTrue(
                    IsTokenType(o, JTokenType.Object),
                    AllOf(schema.Properties.Select(i =>
                        BuildProperty(builder, i.Key, i.Value, Expression.Convert(o, typeof(JObject))))));

            if (schema.PatternProperties.Count > 0)
                yield return IfThenElseTrue(
                    IsTokenType(o, JTokenType.Object),
                        AllOf(schema.PatternProperties.Select(i =>
                            BuildPatternProperty(builder, i.Key, i.Value, Expression.Convert(o, typeof(JObject))))));

            if (schema.AllowAdditionalProperties == false)
            {
                yield return IfThenElseTrue(
                    IsTokenType(o, JTokenType.Object),
                    CallThis(
                        nameof(AllowAdditionalProperties),
                        Expression.Constant(schema),
                        Expression.Convert(o, typeof(JObject))));
            }
            else if (schema.AdditionalProperties != null)
            {
                var p = Expression.Parameter(typeof(JToken));

                yield return IfThenElseTrue(
                    IsTokenType(o, JTokenType.Object),
                    CallThis(
                        nameof(AdditionalProperties),
                        Expression.Constant(schema),
                        Expression.Convert(o, typeof(JObject)),
                        EvalSchemaFunc(builder, schema.AdditionalProperties)));
            }
        }

        Expression BuildProperty(JSchemaExpressionBuilder builder, string propertyName, JSchema propertySchema, Expression o)
        {
            if (o.Type != typeof(JObject))
                throw new ArgumentException(nameof(o));

            return CallThis(nameof(Property), Expression.Constant(propertyName), EvalSchemaFunc(builder, propertySchema), o);
        }

        static bool Property(string propertyName, Func<JToken, bool> propertySchema, JObject o)
        {
            if (o.TryGetValue(propertyName, out var p))
                return propertySchema(p);

            return true;
        }

        Expression BuildPatternProperty(JSchemaExpressionBuilder builder, string propertyPattern, JSchema propertySchema, Expression o)
        {
            if (o.Type != typeof(JObject))
                throw new ArgumentException(nameof(o));

            return CallThis(nameof(PatternProperty), Expression.Constant(propertyPattern), EvalSchemaFunc(builder, propertySchema), o);
        }

        static bool PatternProperty(string propertyPattern, Func<JToken, bool> propertySchema, JObject o)
        {
            foreach (var p in o.Properties())
                if (Regex.IsMatch(p.Name, propertyPattern))
                    if (!propertySchema(p.Value))
                        return false;

            return true;
        }

        static bool AllowAdditionalProperties(JSchema schema, JObject o)
        {
            foreach (var p in o.Properties())
                if (schema.Properties.ContainsKey(p.Name) == false &&
                    schema.PatternProperties.Any(i => Regex.IsMatch(p.Name, i.Key)) == false)
                    return false;

            return true;
        }

        static bool AdditionalProperties(JSchema schema, JObject o, Func<JToken, bool> additionalPropertiesSchema)
        {
            foreach (var p in o.Properties())
                if (schema.Properties.ContainsKey(p.Name) == false &&
                    schema.PatternProperties.Any(i => Regex.IsMatch(p.Name, i.Key)) == false)
                    if (additionalPropertiesSchema(p.Value) == false)
                        return false;

            return true;
        }

    }

}
