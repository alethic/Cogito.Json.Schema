using System.Linq;
using System.Linq.Expressions;

using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Validation.Builders
{

    public class OneOfExpressionBuilder : ExpressionBuilderBase
    {

        public override Expression Build(JSchemaExpressionBuilder builder, JSchema schema, Expression token)
        {
            if (schema.OneOf.Count == 0)
                return null;

            return OneOf(schema.OneOf.Select(i => builder.Eval(i, token)));
        }

    }

}
