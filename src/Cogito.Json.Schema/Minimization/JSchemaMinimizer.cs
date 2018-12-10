using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization
{

    /// <summary>
    /// Provides methods to reduce <see cref="JSchema"/>.
    /// </summary>
    public class JSchemaMinimizer
    {

        /// <summary>
        /// Default set of reducers.
        /// </summary>
        public static readonly IReadOnlyCollection<JSchemaReducer> DefaultReducers =
            typeof(JSchemaReducingTransformer).Assembly.GetTypes()
                .Where(i => typeof(JSchemaReducer).IsAssignableFrom(i))
                .Where(i => i.IsAbstract == false)
                .Select(i => (JSchemaReducer)Activator.CreateInstance(i))
                .ToArray();

        readonly List<JSchemaReducer> reducers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reducers"></param>
        public JSchemaMinimizer(IEnumerable<JSchemaReducer> reducers)
        {
            this.reducers = new List<JSchemaReducer>(reducers);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public JSchemaMinimizer() :
            this(DefaultReducers)
        {

        }

        /// <summary>
        /// Gets the set of reductions to use when reducing.
        /// </summary>
        public ICollection<JSchemaReducer> Reducers => reducers;

        /// <summary>
        /// Reduces the given <see cref="JSchema"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public JSchema Minimize(JSchema schema)
        {
            return new JSchemaReducingTransformer(reducers).Transform(schema);
        }

    }

}
