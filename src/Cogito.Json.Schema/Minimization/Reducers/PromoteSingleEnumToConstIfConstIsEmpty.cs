using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    public class PromoteSingleEnumToConstIfConstIsEmpty : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.Const == null &&
                schema.Enum.Count == 1)
            {
                schema = schema.Clone();
                schema.Const = schema.Enum[0];
                schema.Enum.Clear();
            }

            return schema;
        }

    }

}

