using System.Collections.Generic;

using Cogito.Collections;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    class RemoveDuplicateAllOf : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.AllOf.Count > 0)
            {
                var l = new List<JSchema>();
                var h = new HashSet<JToken>(new JTokenEqualityComparer());

                foreach (var i in schema.AllOf)
                    if (h.Add(i.ToJToken()))
                        l.Add(i);

                // number of items were changed
                if (l.Count != schema.AllOf.Count)
                {
                    schema = schema.Clone();
                    schema.AllOf.Clear();
                    schema.AllOf.AddRange(l);
                }
            }

            return schema;
        }

    }

}
