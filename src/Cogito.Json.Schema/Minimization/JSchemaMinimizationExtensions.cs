using System.Collections.Generic;

using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization
{

    /// <summary>
    /// Extension methods to minimize <see cref="JSchema"/>.
    /// </summary>
    public static class JSchemaMinimizationExtensions
    {

        /// <summary>
        /// Minimizes the JSON schema.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static JSchema Minimize(this JSchema schema)
        {
            return schema != null ? new JSchemaMinimizer().Minimize(schema) : null;
        }

        /// <summary>
        /// Minimizes the JSON schema using the specified reducers.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="reducers"></param>
        /// <returns></returns>
        public static JSchema Minimize(this JSchema schema, IEnumerable<JSchemaReducer> reducers)
        {
            return schema != null ? new JSchemaMinimizer(reducers).Minimize(schema) : null;
        }

    }

}
