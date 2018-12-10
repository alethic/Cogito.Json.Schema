using Cogito.Collections;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    /// <summary>
    /// Schema contains no oneOf references, but does contain a single allOf reference, copy to oneOf.
    /// </summary>
    class PromoteAllOfWithOneOfToOneOfIfOneOfIsEmpty : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.OneOf.Count == 0 &&
                schema.AllOf.Count == 1 &&
                schema.AllOf[0].OneOf.Count > 0)
            {
                if (schema.AllOf[0].ToJObject().Count == 1 ||
                    schema.AllOf[0].ToJObject().Count == 2 && schema.AllOf[0].Type == schema.Type)
                {
                    schema = schema.Clone();
                    schema.OneOf.AddRange(schema.AllOf[0].OneOf);
                    schema.AllOf.Clear();
                }
            }

            return schema;
        }

    }

}
