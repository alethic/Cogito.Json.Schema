using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    /// <summary>
    /// Remove a type expression if the schema is a const with a matching type.
    /// </summary>
    class RemoveTypeWithConst : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.Const != null &&
                schema.Type != null)
            {
                var t = TypeFromToken(schema.Const);
                if (t != null &&
                    t == schema.Type)
                {
                    schema = schema.Clone();
                    schema.Type = null;
                }
            }

            return schema;
        }

        /// <summary>
        /// Returns the known schema type from the given token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        JSchemaType? TypeFromToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Array:
                    return JSchemaType.Array;
                case JTokenType.Boolean:
                    return JSchemaType.Boolean;
                case JTokenType.Float:
                    return JSchemaType.Number;
                case JTokenType.Integer:
                    return JSchemaType.Integer;
                case JTokenType.Null:
                    return JSchemaType.Null;
                case JTokenType.Object:
                    return JSchemaType.Object;
                case JTokenType.String:
                case JTokenType.Date:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.TimeSpan:
                case JTokenType.Uri:
                    return JSchemaType.String;
                default:
                    return null;
            }
        }

    }

}
