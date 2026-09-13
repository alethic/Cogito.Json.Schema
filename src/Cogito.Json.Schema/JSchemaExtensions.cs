
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema
{

    /// <summary>
    /// Extensions for working with <see cref="JSchema"/> instances.
    /// </summary>
    public static class JSchemaExtensions
    {

        static readonly JSchemaCopyTransformer copy = new JSchemaCopyTransformer();

        /// <summary>
        /// Creates a copy of the given <see cref="JSchema"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static JSchema Clone(this JSchema schema)
        {
            return schema != null ? copy.Transform(schema) : null;
        }

        /// <summary>
        /// Converts a <see cref="JSchema"/> to a <see cref="JObject"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static JToken ToJToken(this JSchema schema)
        {
            return schema != null ? JToken.FromObject(schema) : null;
        }

        /// <summary>
        /// Converts a <see cref="JSchema"/> to a <see cref="JObject"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static JObject ToJObject(this JSchema schema)
        {
            return schema != null ? JObject.FromObject(schema) : null;
        }

    }

}
