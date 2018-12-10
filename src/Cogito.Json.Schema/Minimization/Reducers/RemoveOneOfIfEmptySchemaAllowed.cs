using System.Linq;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    /// <summary>
    /// Removes the entire oneOf if the oneOf allows an empty schema: everything would validate
    /// </summary>
    class RemoveOneOfIfEmptySchemaAllowed : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.OneOf.Any(i => i.Valid == true || i.ToJToken() is JObject o && o.Count == 0))
            {
                schema = schema.Clone();
                schema.OneOf.Clear();
            }

            return schema;
        }

    }

}
