namespace Cogito.Json.Schema.Validation
{

    /// <summary>
    /// Provides configuration to the JSON Schema Expression Builder.
    /// </summary>
    public class JSchemaExpressionBuilderOptions
    {

        /// <summary>
        /// Indicates whether validation of the 'format' keyword will be enabled. By default this is determined by the JSON schema version being validated.
        /// </summary>
        public bool? ValidateFormat { get; set; }

    }

}
