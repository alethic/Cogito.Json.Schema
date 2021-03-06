﻿using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema
{

    /// <summary>
    /// Applies each configured transform function until no modifications are made.
    /// </summary>
    public class JSchemaDelegatingTransformer :
        JSchemaTransformer
    {

        /// <summary>
        /// Transforms the schema according to the specified transforms.
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static JSchema Transform(IEnumerable<Func<JSchema, JSchema>> transforms, JSchema schema)
        {
            return new JSchemaDelegatingTransformer(transforms).Transform(schema);
        }

        readonly List<Func<JSchema, JSchema>> transforms;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="transforms"></param>
        public JSchemaDelegatingTransformer(IEnumerable<Func<JSchema, JSchema>> transforms)
        {
            this.transforms = transforms?.ToList() ?? throw new ArgumentNullException(nameof(transforms));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="transforms"></param>
        public JSchemaDelegatingTransformer(params Func<JSchema, JSchema>[] transforms) :
            this(transforms.AsEnumerable())
        {

        }

        /// <summary>
        /// Gets the set of transforms to be applied.
        /// </summary>
        public IList<Func<JSchema, JSchema>> Transforms => transforms;

        /// <summary>
        /// Transforms the specified <see cref="JSchema"/> through each configured transformer until no more modifications are made.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public override JSchema Transform(JSchema schema)
        {
            // nothing to possibly transform
            if (schema == null)
                return null;

            // depth first
            var s1 = base.Transform(schema);
            if (s1 == null)
                return null;

            // apply transforms
            for (int i = 0; i < transforms.Count; i++)
            {
                var r = transforms[i];
                if (r == null)
                    continue;

                // apply transforms
                var s2 = r(s1);

                // transforms resulted in no changes
                if (!JSchemaEqualityComparer.Default.Equals(s1, s2))
                {
                    // start over using changed
                    s1 = s2;
                    i = -1;
                }
            }

            // resulting schema has been fully transformed
            return s1;
        }

    }

}
