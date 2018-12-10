using System;
using System.Diagnostics;
using System.IO;

using Cogito.Json.Schema.Validation;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Tests
{

    [TestClass]
    public class JSchemaValidatorBuilderTests
    {

        [TestMethod]
        public void Should_validate_const_integer()
        {
            var s = new JSchema() { Const = 1 };
            var o = new JValue(1);
            var r = JSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Should_fail_to_validate_const_integer()
        {
            var s = new JSchema() { Const = 1 };
            var o = new JValue(2);
            var r = JSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeFalse();
        }

        [TestMethod]
        public void Should_validate_single_property_with_const()
        {
            var s = new JSchema() { Properties = { ["Prop"] = new JSchema() { Const = 1 } } };
            var o = new JObject() { ["Prop"] = 1 };
            var r = JSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Should_fail_to_validate_single_property_with_const()
        {
            var s = new JSchema() { Properties = { ["Prop"] = new JSchema() { Const = 1 } } };
            var o = new JObject() { ["Prop"] = 2 };
            var r = JSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeFalse();
        }

        [TestMethod]
        public void Should_skip_validating_single_property_with_const()
        {
            var s = new JSchema() { Properties = { ["Prop1"] = new JSchema() { Const = 1 } } };
            var o = new JObject() { ["Prop2"] = 2 };
            var r = JSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Should_validate_recursive_ref()
        {
            var s = JSchema.Parse("{ 'properties': { 'Prop1': { '$ref': '#' }, 'Prop2': { 'const': 'value' } } }");
            var o = new JObject() { ["Prop1"] = new JObject() { ["Prop1"] = null } };
            var r = JSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Can_load_really_big_schema()
        {
            var s = JSchema.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(JSchemaValidatorBuilderTests).Assembly.Location), "Schema", "ecourt_com_151.json")));

            var sdkf = typeof(JSchemaValidatorBuilderTests).Assembly.GetManifestResourceStream("Cogito.Json.Schema.Tests.efm.json");
            var o = JObject.ReadFrom(new JsonTextReader(new StreamReader(sdkf)));


            var v = JSchemaExpressionBuilder.CreateDefault().Build(s);

            //BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            //PropertyInfo debugViewProp = typeof(Expression).GetProperty("DebugView", flags);
            //MethodInfo debugViewGetter = debugViewProp.GetGetMethod(nonPublic: true);
            //string debugView = (string)debugViewGetter.Invoke(v, null);

            var a = v.Compile();
            a.Invoke(o);
            var sw = new Stopwatch();

            var t = TimeSpan.Zero;
            for (var i = 0; i < 1000; i++)
            {
                sw.Start();
                var r = a.Invoke(o);
                sw.Stop();
                t += sw.Elapsed;
                sw.Reset();
            }
            Console.WriteLine("Average on Fast Validator: " + new TimeSpan((long)(t.Ticks / 1000d)));


            t = TimeSpan.Zero;
            for (var i = 0; i < 1000; i++)
            {
                sw.Start();
                var r = o.IsValid(s);
                sw.Stop();
                t += sw.Elapsed;
                sw.Reset();
            }
            Console.WriteLine("Average on Slow Validator: " + new TimeSpan((long)(t.Ticks / 1000d)));
        }

    }

}
