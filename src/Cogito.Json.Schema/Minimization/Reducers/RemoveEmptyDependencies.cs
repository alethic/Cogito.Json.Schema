using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    /// <summary>
    /// Removes empty 'dependencies' from the schema.
    /// </summary>
    class RemoveEmptyDependencies : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            // if valid is set, serialization is fine
            if (schema.Valid != null)
                return schema;

            // find empty dependencies property and remove
            if (schema.ToJToken() is JObject o &&
                o.Property("dependencies") is JProperty p &&
                p.Value is JObject v &&
                v.HasValues == false)
            {
                o.Remove("dependencies");
                return JSchema.Load(o.CreateReader());
            }

            // original schema was fine
            return schema;
        }

    }

}
