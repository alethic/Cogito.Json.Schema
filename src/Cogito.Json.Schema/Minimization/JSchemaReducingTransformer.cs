using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization
{

    /// <summary>
    /// Transformer implementation that applies a series of reductions to a <see cref="JSchema"/>.
    /// </summary>
    class JSchemaReducingTransformer
    {

        readonly JSchemaDelegatingTransformer transformer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reducers"></param>
        public JSchemaReducingTransformer(IList<JSchemaReducer> reducers) :
            base()
        {
            this.transformer = new JSchemaDelegatingTransformer(reducers?.Select<JSchemaReducer, Func<JSchema, JSchema>>(i => i.Reduce));
        }

        public JSchema Transform(JSchema schema)
        {
            return transformer.Transform(schema);
        }

    }

}
