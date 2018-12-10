using System.Collections.Generic;

using Cogito.Collections;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    class RemoveDuplicateOneOf : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.OneOf.Count > 0)
            {
                var l = new List<JSchema>();
                var h = new HashSet<JToken>(new JTokenEqualityComparer());

                foreach (var i in schema.OneOf)
                    if (h.Add(i.ToJToken()))
                        l.Add(i);

                // number of items were changed
                if (l.Count != schema.OneOf.Count)
                {
                    schema = schema.Clone();
                    schema.OneOf.Clear();
                    schema.OneOf.AddRange(l);
                }
            }

            return schema;
        }

    }

}
