using System.Collections.Generic;
using System.Linq;

using Cogito.Collections;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    public class RemoveDuplicateEnum : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.Enum.Count > 0)
            {
                var h = new HashSet<JToken>(new JTokenEqualityComparer());
                foreach (var i in schema.Enum)
                    h.Add(i);

                if (h.Count != schema.Enum.Count)
                {
                    schema = schema.Clone();
                    schema.Enum.Clear();
                    schema.Enum.AddRange(h);
                }
            }

            return schema;
        }

    }

}
