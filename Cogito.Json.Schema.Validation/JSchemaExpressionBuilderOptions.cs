namespace Cogito.Json.Schema.Validation
{

    /// <summary>
    /// Provides configuration to the JSON Schema Expression Builder.
    /// </summary>
    public class JSchemaExpressionBuilderOptions
    {

        /// <summary>
        /// Indicates whether assertion of the 'content' keywords will be enabled. By default this is determined by the JSON schema version being validated.
        /// </summary>
        public bool? TreatContentAsAssertion { get; set; }

        /// <summary>
        /// Indicates whether assertion of the 'format' keyword will be enabled. By default this is determined by the JSON schema version being validated.
        /// </summary>
        public bool? TreatFormatAsAssertion { get; set; }

    }

}
