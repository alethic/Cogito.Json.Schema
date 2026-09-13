using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Tests
{

    [TestClass]
    public class JSchemaEqualityComparerTests
    {

        [TestMethod]
        public void Should_return_true_for_matching_valid()
        {
            var x = new JSchema()
            {
                Valid = true,
            };

            var y = new JSchema()
            {
                Valid = true
            };

            JSchemaEqualityComparer.Default.Equals(x, y).Should().BeTrue();
        }

        [TestMethod]
        public void Should_return_false_for_mismatched_valid()
        {
            var x = new JSchema()
            {
                Valid = true,
            };

            var y = new JSchema()
            {
                Valid = false
            };

            JSchemaEqualityComparer.Default.Equals(x, y).Should().BeFalse();
        }

    }

}
