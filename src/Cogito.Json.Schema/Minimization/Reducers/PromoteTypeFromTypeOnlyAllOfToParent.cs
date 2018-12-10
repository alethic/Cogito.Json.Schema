using System.Linq;

using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    class PromoteTypeFromTypeOnlyAllOfToParent : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.Type == null &&
                schema.AllOf.Count > 0)
            {
                var t = schema.AllOf.Where(i => i.Type != null && i.ToJObject().Count == 1).FirstOrDefault();
                if (t != null)
                {
                    schema = schema.Clone();
                    schema.Type = t.Type;
                }
            }

            return schema;
        }

    }

}
