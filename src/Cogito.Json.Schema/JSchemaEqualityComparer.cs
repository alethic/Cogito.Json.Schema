using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema
{

    /// <summary>
    /// Implements an equality comparer for <see cref="JSchema"/> instances.
    /// </summary>
    public class JSchemaEqualityComparer : IEqualityComparer<JSchema>
    {

        public static readonly JSchemaEqualityComparer Default = new JSchemaEqualityComparer();

        readonly IEqualityComparer<JToken> tokenComparer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="tokenComparer"></param>
        public JSchemaEqualityComparer(IEqualityComparer<JToken> tokenComparer)
        {
            this.tokenComparer = tokenComparer ?? throw new ArgumentNullException(nameof(tokenComparer));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public JSchemaEqualityComparer() :
            this(new JTokenEqualityComparer())
        {

        }

        public bool Equals(JSchema x, JSchema y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x != null && y == null ||
                y != null && x == null)
                return false;

            var b = true;

            b &= Equals(x.AdditionalItems, y.AdditionalItems);
            b &= Equals(x.AdditionalProperties, y.AdditionalProperties);
            b &= x.AllOf.SequenceEqual(y.AllOf, this);
            b &= x.AllowAdditionalItems == y.AllowAdditionalItems;
            b &= x.AllowAdditionalProperties == y.AllowAdditionalProperties;
            b &= x.AnyOf.SequenceEqual(y.AnyOf, this);
            b &= tokenComparer.Equals(x.Const, y.Const);
            b &= Equals(x.Contains, y.Contains);
            b &= x.ContentEncoding == y.ContentEncoding;
            b &= x.ContentMediaType == y.ContentMediaType;
            b &= tokenComparer.Equals(x.Default, y.Default);
            b &= Equals(x.Dependencies, y.Dependencies, EqualityComparer<object>.Default);
            b &= x.Description == y.Description;
            b &= Equals(x.Else, y.Else);
            b &= x.Enum.SequenceEqual(y.Enum, tokenComparer);
            b &= x.ExclusiveMaximum == y.ExclusiveMaximum;
            b &= x.ExclusiveMinimum == y.ExclusiveMinimum;
            b &= Equals(x.ExtensionData, y.ExtensionData, tokenComparer);
            b &= x.Format == y.Format;
            b &= x.Id == y.Id;
            b &= Equals(x.If, y.If);
            b &= x.Items.SequenceEqual(y.Items, this);
            b &= x.ItemsPositionValidation == y.ItemsPositionValidation;
            b &= x.Maximum == y.Maximum;
            b &= x.MaximumItems == y.MaximumItems;
            b &= x.MaximumLength == y.MaximumLength;
            b &= x.MaximumProperties == y.MaximumProperties;
            b &= x.Minimum == y.Minimum;
            b &= x.MinimumItems == y.MinimumItems;
            b &= x.MinimumLength == y.MinimumLength;
            b &= x.MinimumProperties == y.MinimumProperties;
            b &= x.MultipleOf == y.MultipleOf;
            b &= Equals(x.Not, y.Not);
            b &= x.OneOf.SequenceEqual(y.OneOf, this);
            b &= x.Pattern == y.Pattern;
            b &= Equals(x.PatternProperties, y.PatternProperties, this);
            b &= Equals(x.Properties, y.Properties, this);
            b &= Equals(x.PropertyNames, y.PropertyNames);
            b &= x.ReadOnly == y.ReadOnly;
            b &= x.Reference == y.Reference;
            b &= x.Required.SequenceEqual(y.Required);
            b &= x.SchemaVersion == y.SchemaVersion;
            b &= Equals(x.Then, y.Then);
            b &= x.Title == y.Title;
            b &= x.Type == y.Type;
            b &= x.UniqueItems == y.UniqueItems;
            b &= x.Valid == y.Valid;
            b &= x.WriteOnly == y.WriteOnly;

            return b;
        }

        /// <summary>
        /// Compares two dictionaries for equality.
        /// </summary
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="valueComparer"></param>
        /// <returns></returns>
        bool Equals<K, V>(IDictionary<K, V> a, IDictionary<K, V> b, IEqualityComparer<V> valueComparer)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a == null && b != null ||
                a != null && b == null)
                return false;

            if (a.Count != b.Count)
                return false;

            foreach (var kvp in a)
                if (b.TryGetValue(kvp.Key, out var bv) == false || valueComparer.Equals(kvp.Value, bv) == false)
                    return false;

            return true;
        }

        public int GetHashCode(JSchema obj)
        {
            if (obj is null)
                return 0;

            var h = new HashCode();
            h.Add(GetHashCode(obj.AdditionalItems));
            h.Add(GetHashCode(obj.AdditionalProperties));
            h.Add(GetHashCode(obj.AllOf, this));
            h.Add(obj.AllowAdditionalItems);
            h.Add(obj.AllowAdditionalProperties);
            h.Add(GetHashCode(obj.AnyOf, this));
            h.Add(tokenComparer.GetHashCode(obj.Const));
            h.Add(GetHashCode(obj.Contains));
            h.Add(obj.ContentEncoding);
            h.Add(obj.ContentMediaType);
            h.Add(tokenComparer.GetHashCode(obj.Default));
            h.Add(GetHashCode(obj.Dependencies, EqualityComparer<object>.Default));
            h.Add(obj.Description);
            h.Add(GetHashCode(obj.Else));
            h.Add(GetHashCode(obj.Enum, tokenComparer));
            h.Add(obj.ExclusiveMaximum);
            h.Add(obj.ExclusiveMinimum);
            h.Add(GetHashCode(obj.ExtensionData, tokenComparer));
            h.Add(obj.Format);
            h.Add(obj.Id);
            h.Add(GetHashCode(obj.If));
            h.Add(GetHashCode(obj.Items, this));
            h.Add(obj.ItemsPositionValidation);
            h.Add(obj.Maximum);
            h.Add(obj.MaximumItems);
            h.Add(obj.MaximumLength);
            h.Add(obj.MaximumProperties);
            h.Add(obj.Minimum);
            h.Add(obj.MinimumItems);
            h.Add(obj.MinimumLength);
            h.Add(obj.MinimumProperties);
            h.Add(obj.MultipleOf);
            h.Add(GetHashCode(obj.Not));
            h.Add(GetHashCode(obj.OneOf, this));
            h.Add(obj.Pattern);
            h.Add(GetHashCode(obj.PatternProperties, this));
            h.Add(GetHashCode(obj.Properties, this));
            h.Add(GetHashCode(obj.PropertyNames));
            h.Add(obj.ReadOnly);
            h.Add(obj.Reference);
            h.Add(GetHashCode(obj.Required, StringComparer.Ordinal));
            h.Add(obj.SchemaVersion);
            h.Add(GetHashCode(obj.Then));
            h.Add(obj.Title);
            h.Add(obj.Type);
            h.Add(obj.UniqueItems);
            h.Add(obj.Valid);
            h.Add(obj.WriteOnly);

            return h.ToHashCode();
        }

        int GetHashCode<T>(IEnumerable<T> list, IEqualityComparer<T> comparer)
        {
            if (list == null)
                return 0;

            var h = new HashCode();

            foreach (var i in list)
                h.Add(comparer.GetHashCode(i));

            return h.ToHashCode();
        }

        int GetHashCode<K, V>(IDictionary<K, V> a, IEqualityComparer<V> valueComparer)
        {
            var h = new HashCode();

            foreach (var kvp in a.OrderBy(i => i.Key))
            {
                h.Add(kvp.Value.GetHashCode());
                h.Add(valueComparer.GetHashCode(kvp.Value));
            }

            return h.ToHashCode();
        }

    }

}
