using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization
{

    /// <summary>
    /// Implementation of a particular reducer algorithm.
    /// </summary>
    public abstract class JSchemaReducer
    {

        /// <summary>
        /// Produces a reduction of the given <see cref="JSchema"/>, if possible. Implementations should try to return
        /// the <see cref="JSchema"/> instance if no changes are to be applied, but must ensure they create a clone of
        /// the instance in case of changes.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public abstract JSchema Reduce(JSchema schema);

    }

}
