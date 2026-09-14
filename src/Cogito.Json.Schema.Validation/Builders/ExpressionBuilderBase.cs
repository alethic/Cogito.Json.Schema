using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Validation.Builders
{

    public abstract class ExpressionBuilderBase : IExpressionBuilder
    {

        public abstract Expression Build(JSchemaExpressionBuilder builder, JSchema schema, Expression token);

        protected static readonly Expression True = Expression.Constant(true);
        protected static readonly Expression False = Expression.Constant(false);
        protected static readonly Expression Null = Expression.Constant(null);

        /// <summary>
        /// Returns an expression that returns a delegate to evaluate the schema.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        protected Expression EvalSchemaFunc(JSchemaExpressionBuilder builder, JSchema schema)
        {
            var p = Expression.Parameter(typeof(JToken));
            return Expression.Lambda<Func<JToken, bool>>(builder.Eval(schema, p), p);
        }

        /// <summary>
        /// Returns an expression that returns <c>true</c> if all of the given expressions returns <c>true</c>.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        protected static Expression AllOf(IEnumerable<Expression> expressions)
        {
            return expressions.Aggregate(True, (a, b) => Expression.AndAlso(a, b));
        }

        /// <summary>
        /// Returns an expression that returns <c>true</c> if any of the given expressions returns <c>true</c>.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        protected static Expression AnyOf(IEnumerable<Expression> expressions)
        {
            return expressions.Aggregate(False, (a, b) => Expression.OrElse(a, b));
        }

        /// <summary>
        /// Returns an expression that returns <c>true</c> if one of the given expressions returns <c>true</c>.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        protected static Expression OneOf(IEnumerable<Expression> expressions)
        {
            var rsl = Expression.Variable(typeof(bool));
            var brk = Expression.Label(typeof(bool));

            return Expression.Block(
                new[] { rsl },
                Expression.Block(
                    expressions.Select(i =>
                        Expression.IfThen(i,
                            Expression.IfThenElse(rsl,
                                Expression.Return(brk, False),
                                Expression.Assign(rsl, True))))),
                Expression.Label(brk, rsl));
        }

        /// <summary>
        /// Returns an expression that calls the given method on this class.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected Expression CallThis(string methodName, params Expression[] args) =>
            Expression.Call(
                GetType().GetMethod(methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public),
                args);

        /// <summary>
        /// Returns an expression that returns <c>true</c> if JSON token type of the specified expression.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        protected static Expression TokenType(Expression o) =>
            Expression.Property(o, typeof(JToken).GetProperty(nameof(JToken.Type)));

        /// <summary>
        /// Returns an expression that returns <c>true</c> if the specified expression of the the given token type.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static Expression IsTokenType(Expression o, JTokenType type) =>
            Expression.Equal(TokenType(o), Expression.Constant(type));

        /// <summary>
        /// Returns an expression that returns <c>true</c> if the given test is false, else executes the <paramref name="ifTrue"/> condition.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="ifTrue"></param>
        /// <returns></returns>
        protected static Expression IfThenElseTrue(Expression test, Expression ifTrue) =>
            Expression.Condition(test, ifTrue, True);

    }

}
