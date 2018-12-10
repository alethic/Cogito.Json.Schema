using Cogito.Json.Schema.Minimization;

using FluentAssertions;
using FluentAssertions.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Tests.Minimization
{

    [TestClass]
    public class JSchemaMinimizerTests
    {

        [TestMethod]
        public void Should_not_alter_single_const()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Const = "123"
            });

            var t = new JSchema()
            {
                Const = "123"
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_duplicate_allof()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Const = "Bar",
                    },
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Const = "Bar",
                    }
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_duplicate_anyof()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                AnyOf =
                {
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Const = "Bar",
                    },
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
                AnyOf =
                {
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Const = "Bar",
                    }
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_duplicate_enum()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                Enum = { "A", "B", "C", "C" }
            });

            var t = new JSchema()
            {
                Title = "Test",
                Enum = { "A", "B", "C" }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_duplicate_oneof()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                OneOf =
                {
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Const = "Bar",
                    },
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
                OneOf =
                {
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Const = "Bar",
                    }
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_promote_allof_with_oneof_to_oneof_if_oneof_is_empty()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema()
                    {
                        OneOf =
                        {
                            new JSchema()
                            {
                                Const = "Foo",
                            },
                            new JSchema()
                            {
                                Const = "Bar",
                            },
                        }
                    }
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
                OneOf =
                {
                    new JSchema()
                    {
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Const = "Bar",
                    }
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_promote_allof_inside_allof_to_parent()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema()
                    {
                        Title = "A",
                        AllOf =
                        {
                            new JSchema()
                            {
                                Title = "B",
                                Const = "DO NOT MOVE"
                            }
                        }
                    },
                    new JSchema()
                    {
                        AllOf =
                        {
                            new JSchema()
                            {
                                Title = "C",
                                Const = "Foo",
                            },
                            new JSchema()
                            {
                                Title = "D",
                                Const = "Bar"
                            },
                        }
                    }
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema()
                    {
                        Title = "A",
                        AllOf =
                        {
                            new JSchema()
                            {
                                Title = "B",
                                Const = "DO NOT MOVE"
                            }
                        }
                    },
                    new JSchema()
                    {
                        Title = "C",
                        Const = "Foo",
                    },
                    new JSchema()
                    {
                        Title = "D",
                        Const = "Bar"
                    },
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_impossible_enum_solutions_if_const_specified()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                Const = "BOB",
                Enum = { "A", "BOB" }
            });

            var t = new JSchema()
            {
                Title = "Test",
                Const = "BOB"
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_empty_schema_from_allof()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema(),
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_oneOf_if_empty_schema_allowed()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                OneOf =
                {
                    new JSchema(),
                    new JSchema()
                    {
                        Title = "Foo"
                    }
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_allof_if_only_item_is_empty_schema()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema(),
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_promote_type_from_allof_to_parent_if_not_specified()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                AllOf =
                {
                    new JSchema()
                    {
                        Type = JSchemaType.String
                    }
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
                Type = JSchemaType.String
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_allof_item_with_same_type_as_parent()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Title = "Test",
                Type = JSchemaType.String,
                AllOf =
                {
                    new JSchema()
                    {
                        Type = JSchemaType.String
                    }
                }
            });

            var t = new JSchema()
            {
                Title = "Test",
                Type = JSchemaType.String
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_promote_allof_with_duplicate_type()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Type = JSchemaType.String,
                AllOf =
                {
                    new JSchema()
                    {
                        Type = JSchemaType.String
                    },
                    new JSchema()
                    {
                        AllOf =
                        {
                            new JSchema()
                            {
                                Const = "Value"
                            },
                            new JSchema()
                            {
                                Const = "Value2"
                            }
                        }
                    }
                }
            });

            var t = new JSchema()
            {
                Type = JSchemaType.String,
                AllOf =
                {
                    new JSchema()
                    {
                        Const = "Value"
                    },
                    new JSchema()
                    {
                        Const = "Value2"
                    }
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_promote_oneof_in_nested_allof_and_duplicate_type()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Type = JSchemaType.String,
                AllOf =
                {
                    new JSchema()
                    {
                        Type = JSchemaType.String
                    },
                    new JSchema()
                    {
                        OneOf =
                        {
                            new JSchema()
                            {
                                Const = "Value"
                            },
                            new JSchema()
                            {
                                Const = "Value2"
                            }
                        }
                    }
                }
            });

            var t = new JSchema()
            {
                Type = JSchemaType.String,
                OneOf =
                {
                    new JSchema()
                    {
                        Const = "Value"
                    },
                    new JSchema()
                    {
                        Const = "Value2"
                    }
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_promote_oneof_in_nested_allof_if_allof_type_matches()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Type = JSchemaType.String,
                AllOf =
                {
                    new JSchema()
                    {
                        Type = JSchemaType.String,
                        OneOf =
                        {
                            new JSchema()
                            {
                                Const = "Value1"
                            },
                            new JSchema()
                            {
                                Const = "Value2"
                            }
                        }
                    }
                }
            });

            var t = new JSchema()
            {
                Type = JSchemaType.String,
                OneOf =
                {
                    new JSchema()
                    {
                        Const = "Value1"
                    },
                    new JSchema()
                    {
                        Const = "Value2"
                    }
                }
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_remove_type_if_const()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Type = JSchemaType.String,
                Const = "HELLO",
            });

            var t = new JSchema()
            {
                Const = "HELLO",
            };

            JToken.DeepEquals(s.ToJObject(), t.ToJObject()).Should().BeTrue();
        }

        [TestMethod]
        public void Should_not_fail_on_invalid_schema()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Valid = false
            });
        }

        [TestMethod]
        public void Should_not_fail_on_allof_invalid_schema()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                AllOf =
                {
                    new JSchema()
                    {
                        Valid = false,
                    }
                }
            });
        }

        [TestMethod]
        public void Should_not_fail_on_anyof_invalid_schema()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                AnyOf =
                {
                    new JSchema()
                    {
                        Valid = false,
                    }
                }
            });
        }

        [TestMethod]
        public void Should_not_fail_on_oneof_invalid_schema()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                OneOf =
                {
                    new JSchema()
                    {
                        Valid = false,
                    }
                }
            });
        }

        [TestMethod]
        public void Should_not_fail_on_if_invalid_schema()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                If = new JSchema()
                {
                    Valid = true,
                },
                Then = new JSchema()
                {
                    Valid = false
                },
                Else = new JSchema()
                {
                    Valid = true
                }
            });
        }

        [TestMethod]
        public void Should_not_fail_on_not_invalid_schema()
        {
            var s = new JSchemaMinimizer().Minimize(new JSchema()
            {
                Not = new JSchema()
                {
                    Not = new JSchema()
                    {
                        Valid = true,
                    }
                }
            });
        }

    }

}
