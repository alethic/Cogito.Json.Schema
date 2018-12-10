using System;
using System.Collections.Generic;
using System.Linq;

using Cogito.Collections;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema
{

    /// <summary>
    /// Provides methods to implement a transformer of <see cref="JSchema"/> objects.
    /// </summary>
    public abstract class JSchemaTransformer :
        JSchemaTransformer<object>
    {



    }

    /// <summary>
    /// Provides methods to implement a transformer of <see cref="JSchema"/> objects.
    /// </summary>
    public abstract class JSchemaTransformer<TContext>
    {

        /// <summary>
        /// Transforms a <see cref="JSchema"/> node. Default implementation dispatches to Transform methods for each sub-item and returns a copy of the original object.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public virtual JSchema Transform(JSchema schema)
        {
            return Transform(schema, default);
        }

        /// <summary>
        /// Transforms a <see cref="JSchema"/> node. Default implementation dispatches to Transform methods for each sub-item and returns a copy of the original object.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public virtual JSchema Transform(JSchema schema, TContext context)
        {
            if (schema == null)
                return null;

            var s = new JSchema();
            s.AdditionalItems = TransformAdditionalItems(schema, schema.AdditionalItems, context);
            s.AdditionalProperties = TransformAdditionalProperties(schema, schema.AdditionalProperties, context);
            s.AllOf.AddRange(TransformAllOf(schema, schema.AllOf, context));
            s.AllowAdditionalItems = TransformAllowAdditionalItems(schema, schema.AllowAdditionalItems, context);
            s.AllowAdditionalProperties = TransformAllowAdditionalProperties(schema, schema.AllowAdditionalProperties, context);
            s.AnyOf.AddRange(TransformAnyOf(schema, schema.AnyOf, context));
            s.Const = TransformConst(schema, schema.Const, context);
            s.Contains = TransformContains(schema, schema.Contains, context);
            s.Default = TransformDefault(schema, schema.Default, context);
            s.Dependencies.AddRange(TransformDependencies(schema, schema.Dependencies, context));
            s.Description = TransformDescription(schema, schema.Description, context);
            s.Enum.AddRange(TransformEnum(schema, schema.Enum, context));
            s.ExclusiveMaximum = TransformExclusiveMaximum(schema, schema.ExclusiveMaximum, context);
            s.ExclusiveMinimum = TransformExclusiveMinimum(schema, schema.ExclusiveMinimum, context);
            s.ExtensionData.AddRange(TransformExtensionData(schema, schema.ExtensionData, context));
            s.Format = TransformFormat(schema, schema.Format, context);
            s.Id = TransformId(schema, schema.Id, context);
            s.Items.AddRange(TransformItems(schema, schema.Items, context));
            s.ItemsPositionValidation = TransformItemsPositionValidation(schema, schema.ItemsPositionValidation, context);
            s.Maximum = TransformMaximum(schema, schema.Maximum, context);
            s.MaximumItems = TransformMaximumItems(schema, schema.MaximumItems, context);
            s.MaximumLength = TransformMaximumLength(schema, schema.MaximumLength, context);
            s.MaximumProperties = TransformMaximumProperties(schema, schema.MaximumProperties, context);
            s.Minimum = TransformMinimum(schema, schema.Minimum, context);
            s.MinimumItems = TransformMinimumItems(schema, schema.MinimumItems, context);
            s.MinimumLength = TransformMinimumLength(schema, schema.MinimumLength, context);
            s.MinimumProperties = TransformMinimumProperties(schema, schema.MinimumProperties, context);
            s.MultipleOf = TransformMultipleOf(schema, schema.MultipleOf, context);
            s.Not = TransformNot(schema, schema.Not, context);
            s.OneOf.AddRange(TransformOneOf(schema, schema.OneOf, context));
            s.Pattern = TransformPattern(schema, schema.Pattern, context);
            s.PatternProperties.AddRange(TransformPatternProperties(schema, schema.PatternProperties, context));
            s.Properties.AddRange(TransformProperties(schema, schema.Properties, context));
            s.PropertyNames = TransformPropertyNames(schema, schema.PropertyNames, context);
            s.Required.AddRange(TransformRequired(schema, schema.Required, context));
            s.SchemaVersion = TransformSchemaVersion(schema, schema.SchemaVersion, context);
            s.Title = TransformTitle(schema, schema.Title, context);
            s.Type = TransformType(schema, schema.Type, context);
            s.UniqueItems = TransformUniqueItems(schema, schema.UniqueItems, context);
            s.Valid = TransformValid(schema, schema.Valid, context);
            s.Validators.AddRange(TransformValidators(schema, schema.Validators, context));
            s.If = TransformIf(schema, schema.If, context);
            s.Then = TransformThen(schema, schema.Then, context);
            s.Else = TransformElse(schema, schema.Else, context);
            return s;
        }

        protected virtual JSchema TransformAdditionalItems(JSchema schema, JSchema additionalItems, TContext context)
        {
            return additionalItems != null ? Transform(additionalItems, context) : null;
        }

        protected virtual JSchema TransformAdditionalProperties(JSchema schema, JSchema additionalProperties, TContext context)
        {
            return additionalProperties != null ? Transform(additionalProperties, context) : null;
        }

        protected virtual IEnumerable<JSchema> TransformAllOf(JSchema schema, IList<JSchema> allOf, TContext context)
        {
            return allOf.Select(i => TransformAllOfSchema(schema, i, context));
        }

        protected virtual JSchema TransformAllOfSchema(JSchema parent, JSchema allOf, TContext context)
        {
            return allOf != null ? Transform(allOf, context) : null;
        }

        protected virtual bool TransformAllowAdditionalItems(JSchema schema, bool allowAdditionalItems, TContext context)
        {
            return allowAdditionalItems;
        }

        protected virtual bool TransformAllowAdditionalProperties(JSchema schema, bool allowAdditionalProperties, TContext context)
        {
            return allowAdditionalProperties;
        }

        protected virtual IEnumerable<JSchema> TransformAnyOf(JSchema schema, IList<JSchema> anyOf, TContext context)
        {
            return anyOf.Select(i => TransformAnyOfSchema(schema, i, context));
        }

        protected virtual JSchema TransformAnyOfSchema(JSchema parent, JSchema anyOf, TContext context)
        {
            return anyOf != null ? Transform(anyOf, context) : null;
        }

        protected virtual JToken TransformConst(JSchema schema, JToken @const, TContext context)
        {
            return @const != null ? TransformToken(schema, @const, context) : null;
        }

        protected virtual JSchema TransformContains(JSchema schema, JSchema contains, TContext context)
        {
            return contains != null ? Transform(contains, context) : null;
        }

        protected virtual JToken TransformDefault(JSchema schema, JToken @default, TContext context)
        {
            return @default != null ? TransformToken(schema, @default, context) : null;
        }

        protected virtual IEnumerable<KeyValuePair<string, object>> TransformDependencies(JSchema schema, IDictionary<string, object> dependencies, TContext context)
        {
            foreach (var kvp in dependencies)
            {
                (var propertyName, var value) = TransformDependency(schema, kvp.Key, kvp.Value, context);
                yield return new KeyValuePair<string, object>(propertyName, value);
            }
        }

        protected virtual (string PropertyName, object Dependency) TransformDependency(JSchema schema, string propertyName, object value, TContext context)
        {
            switch (value)
            {
                case JSchema d:
                    return TransformSchemaDependency(schema, propertyName, d, context);
                case string[] d:
                    return TransformPropertyDependency(schema, propertyName, d, context);
                default:
                    throw new NotSupportedException();
            }
        }

        protected virtual (string PropertyName, IEnumerable<string> Dependencies) TransformPropertyDependency(JSchema schema, string propertyName, string[] dependencies, TContext context)
        {
            return (propertyName, dependencies);
        }

        protected virtual (string PropertyName, JSchema Schema) TransformSchemaDependency(JSchema schema, string propertyName, JSchema dependency, TContext context)
        {
            return (propertyName, dependency != null ? Transform(dependency, context) : null);
        }

        protected virtual string TransformDescription(JSchema schema, string description, TContext context)
        {
            return description;
        }

        protected virtual IEnumerable<JToken> TransformEnum(JSchema schema, IList<JToken> @enum, TContext context)
        {
            return @enum.Select(i => TransformEnumToken(schema, i, context));
        }

        protected virtual JToken TransformEnumToken(JSchema schema, JToken token, TContext context)
        {
            return TransformToken(schema, token, context);
        }

        protected virtual bool TransformExclusiveMaximum(JSchema schema, bool exclusiveMaximum, TContext context)
        {
            return exclusiveMaximum;
        }

        protected virtual bool TransformExclusiveMinimum(JSchema schema, bool exclusiveMinimum, TContext context)
        {
            return exclusiveMinimum;
        }

        protected virtual IEnumerable<KeyValuePair<string, JToken>> TransformExtensionData(JSchema schema, IDictionary<string, JToken> extensionData, TContext context)
        {
            return extensionData.Select(i => new KeyValuePair<string, JToken>(i.Key, TransformExtensionData(schema, i.Key, i.Value, context)));
        }

        protected virtual JToken TransformExtensionData(JSchema schema, string name, JToken data, TContext context)
        {
            return data;
        }

        protected virtual string TransformFormat(JSchema schema, string format, TContext context)
        {
            return format;
        }

        protected virtual Uri TransformId(JSchema schema, Uri id, TContext context)
        {
            return id;
        }

        protected virtual IEnumerable<JSchema> TransformItems(JSchema schema, IList<JSchema> items, TContext context)
        {
            return items.Select((i, j) => TransformItem(schema, i, j, context) ?? throw new JSchemaException("Cannot place null schema in items array."));
        }

        protected virtual JSchema TransformItem(JSchema schema, JSchema item, int index, TContext context)
        {
            return Transform(item, context);
        }

        protected virtual bool TransformItemsPositionValidation(JSchema schema, bool itemsPositionValidation, TContext context)
        {
            return itemsPositionValidation;
        }

        protected virtual double? TransformMaximum(JSchema schema, double? maximum, TContext context)
        {
            return maximum;
        }

        protected virtual long? TransformMaximumItems(JSchema schema, long? maximumItems, TContext context)
        {
            return maximumItems;
        }

        protected virtual long? TransformMaximumLength(JSchema schema, long? maximumLength, TContext context)
        {
            return maximumLength;
        }

        protected virtual long? TransformMaximumProperties(JSchema schema, long? maximumProperties, TContext context)
        {
            return maximumProperties;
        }

        protected virtual double? TransformMinimum(JSchema schema, double? minimum, TContext context)
        {
            return minimum;
        }

        protected virtual long? TransformMinimumItems(JSchema schema, long? minimumItems, TContext context)
        {
            return minimumItems;
        }

        protected virtual long? TransformMinimumLength(JSchema schema, long? minimumLength, TContext context)
        {
            return minimumLength;
        }

        protected virtual long? TransformMinimumProperties(JSchema schema, long? minimumProperties, TContext context)
        {
            return minimumProperties;
        }

        protected virtual double? TransformMultipleOf(JSchema schema, double? multipleOf, TContext context)
        {
            return multipleOf;
        }

        protected virtual JSchema TransformNot(JSchema schema, JSchema not, TContext context)
        {
            return not != null ? Transform(not, context) : null;
        }

        protected virtual IEnumerable<JSchema> TransformOneOf(JSchema schema, IList<JSchema> oneOf, TContext context)
        {
            return oneOf.Select(i => TransformOneOfSchema(schema, i, context));
        }

        protected virtual JSchema TransformOneOfSchema(JSchema parent, JSchema oneOf, TContext context)
        {
            return oneOf != null ? Transform(oneOf, context) : null;
        }

        protected virtual string TransformPattern(JSchema schema, string pattern, TContext context)
        {
            return pattern;
        }

        protected virtual IEnumerable<KeyValuePair<string, JSchema>> TransformPatternProperties(JSchema schema, IDictionary<string, JSchema> patternProperties, TContext context)
        {
            return patternProperties;
        }

        protected virtual IEnumerable<KeyValuePair<string, JSchema>> TransformProperties(JSchema schema, IDictionary<string, JSchema> properties, TContext context)
        {
            foreach (var property in properties)
                yield return new KeyValuePair<string, JSchema>(property.Key, TransformProperty(schema, property.Key, property.Value, context));
        }

        protected virtual JSchema TransformProperty(JSchema schema, string name, JSchema property, TContext context)
        {
            return property != null ? Transform(property, context) : null;
        }

        protected virtual JSchema TransformPropertyNames(JSchema schema, JSchema propertyNames, TContext context)
        {
            return propertyNames != null ? Transform(propertyNames, context) : null;
        }

        protected virtual IEnumerable<string> TransformRequired(JSchema schema, IList<string> required, TContext context)
        {
            return new List<string>(required);
        }

        protected virtual Uri TransformSchemaVersion(JSchema schema, Uri schemaVersion, TContext context)
        {
            return schemaVersion;
        }

        protected virtual string TransformTitle(JSchema schema, string title, TContext context)
        {
            return title;
        }

        protected virtual JSchemaType? TransformType(JSchema schema, JSchemaType? type, TContext context)
        {
            return type;
        }

        protected virtual bool TransformUniqueItems(JSchema schema, bool uniqueItems, TContext context)
        {
            return uniqueItems;
        }

        protected virtual bool? TransformValid(JSchema schema, bool? valid, TContext context)
        {
            return valid;
        }

        protected virtual IEnumerable<JsonValidator> TransformValidators(JSchema schema, List<JsonValidator> validators, TContext context)
        {
            return validators.Select(i => TransformValidator(schema, i, context));
        }

        protected virtual JsonValidator TransformValidator(JSchema schema, JsonValidator validator, TContext context)
        {
            return validator;
        }

        protected virtual JToken TransformToken(JSchema schema, JToken token, TContext context)
        {
            return token;
        }

        protected JSchema TransformIf(JSchema schema, JSchema @if, TContext context)
        {
            return @if != null ? Transform(@if, context) : null;
        }

        protected JSchema TransformThen(JSchema schema, JSchema then, TContext context)
        {
            return then != null ? Transform(then, context) : null;
        }

        protected JSchema TransformElse(JSchema schema, JSchema @else, TContext context)
        {
            return @else != null ? Transform(@else, context) : null;
        }

    }

}
