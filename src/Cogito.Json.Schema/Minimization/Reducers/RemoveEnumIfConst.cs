using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Minimization.Reducers
{

    /// <summary>
    /// If const is specified, and enum contains const, then only possible value is const; remove enum
    /// </summary>
    class RemoveEnumIfConst : JSchemaReducer
    {

        public override JSchema Reduce(JSchema schema)
        {
            if (schema.Const != null &&
                schema.Enum.Count > 0 &&
                schema.Enum.Contains(schema.Const))
            {
                schema = schema.Clone();
                schema.Enum.Clear();
            }

            return schema;
        }

    }

}
