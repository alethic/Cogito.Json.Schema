using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Tests.Validation
{

    [TestClass]
    public partial class JSchemaValidatorBuilderTestSuiteTests
    {

        /// <summary>
        /// Resolves from the test suite remote directory
        /// </summary>
        class JSchemaRemoteResolver : JSchemaResolver
        {

            readonly string baseDir;
            readonly JSchemaPreloadedResolver nested;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="baseDir"></param>
            public JSchemaRemoteResolver(string baseDir)
            {
                this.baseDir = baseDir ?? throw new ArgumentNullException(nameof(baseDir));

                nested = new JSchemaPreloadedResolver();
                nested.Add(new Uri("http://json-schema.org/draft-03/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-03.json")));
                nested.Add(new Uri("http://json-schema.org/draft-04/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-04.json")));
                nested.Add(new Uri("http://json-schema.org/draft-06/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-06.json")));
                nested.Add(new Uri("http://json-schema.org/draft-07/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-07.json")));
            }

            public override Stream GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
            {
                if (reference.BaseUri.Host == "localhost")
                    return File.OpenRead(Path.Combine(baseDir, @"JSON-Schema-Test-Suite", "remotes", reference.BaseUri.LocalPath.Trim('/').Replace('/', '\\')));

                return nested.GetSchemaResource(context, reference);
            }

        }

        /// <summary>
        /// Gets the test context.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JSchemaValidatorBuilderTestSuiteTests()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                DateParseHandling = DateParseHandling.None,
            };

            License.RegisterLicense("3572-HcCIRCsOzaHq8+4kTKwIzVMXaM7Vf8rSqUGJM+PwIKMpGa93ep+UImj2opRRQa25zLr33F7nJuEN/0F13kxaquwqKIw8VVjfTRK7TKD1AvuapeWrC/KgiR/bIIqtR9ENHGnaGAYkzsCtJBN5SMbGZmfstOW9hUCRyzi3JXgI/c17IklkIjozNTcyLCJFeHBpcnlEYXRlIjoiMjAxOS0wMS0zMVQyMTowMjowNy43MDk5ODE3WiIsIlR5cGUiOiJKc29uU2NoZW1hU2l0ZSJ9");
        }

        /// <summary>
        /// Resolves from the remote directory.
        /// </summary>
        static JSchemaResolver Resolver { get; } = new JSchemaRemoteResolver(Path.Combine(Path.GetDirectoryName(typeof(JSchemaValidatorBuilderTestSuiteTests).Assembly.Location), "Validation"));

        /// <summary>
        /// Parses the given token string value from base64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static JToken ParseJToken(string value)
        {
            return JToken.ReadFrom(
                new JsonTextReader(new StreamReader(new MemoryStream(Convert.FromBase64String(value))))
                {
                    DateParseHandling = DateParseHandling.None
                });
        }

        /// <summary>
        /// Parses the given schema string value from base64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static JSchema ParseSchema(string value)
        {
            var s = new StreamReader(new MemoryStream(Convert.FromBase64String(value))).ReadToEnd();

            return JSchema.Load(
                new JsonTextReader(new StringReader(s))
                {
                    DateParseHandling = DateParseHandling.None
                },
                Resolver);
        }

    }

}
