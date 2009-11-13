using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqFu
{
    /// <summary>
    /// The main entry point into the LinqFu system.
    /// </summary>
    public class ExpressionBuilder
    {
        private static Expression Clone(Expression expression)
        {
            Expression expressionReturn = null;

            var expressionType = expression.GetType();
            if (expressionType == typeof(ConstantExpression))
            {
                expressionReturn = Clone((ConstantExpression) expression);
            }
            else if (expressionType == typeof(UnaryExpression))
            {
                expressionReturn = Clone((UnaryExpression) expression);
            }
            else if (expressionType == typeof(BinaryExpression))
            {
                expressionReturn = Clone((BinaryExpression) expression);
            }
            else if (expressionType == typeof(ConstantExpression))
            {
                expressionReturn = Clone((ConstantExpression) expression);
            }
            else if (expressionType.IsGenericType && expressionType.GetGenericTypeDefinition() == typeof(Expression<>))
            {
                expressionReturn = expression;
            }

            if (expressionReturn == null) throw new NotSupportedException(String.Format(null, "The supplied expression is not supported '{0}'", expression.GetType().FullName));
            return expressionReturn;
        }

        public static UnaryExpression Clone(UnaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var innerExpression = Clone(expression.Operand);
            var expressionReturn = Expression.MakeUnary(expression.NodeType, innerExpression, expression.Type);
            return expressionReturn;
        }

        public static ConstantExpression Clone(ConstantExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var expressionReturn = Expression.Constant(expression.Value);
            return expressionReturn;
        }

        public static MethodCallExpression Clone(MethodCallExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var arguments = new List<Expression>(expression.Arguments.Count);
            foreach (var argument in expression.Arguments)
            {
                var clone = Clone(argument);
                arguments.Add(clone);
            }
            var expressionReturn = Expression.Call(expression.Method, arguments.ToArray());
            return expressionReturn;
        }

        public static BinaryExpression Clone(BinaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression left = Clone(expression.Left);
            Expression right = Clone(expression.Right);
            var method = expression.Method;

            return null;
        }

        /// <summary>
        /// Converts the supplied <paramref name="expression"/> from <typeparamref name="TPrime"/> to <typeparamref name="TTheta"/>.
        /// </summary>
        /// <typeparam name="TPrime">The type to convert the expression from.</typeparam>
        /// <typeparam name="TTheta">The type to convert the expression to.</typeparam>
        /// <param name="expression">The <see cref="Expression{TDelegate}">expression</see> to convert.</param>
        /// <returns>The converted function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public static Expression<Func<TTheta, Boolean>> Convert<TPrime, TTheta>(Expression<Func<TPrime, Boolean>> expression) // where TTheta : TPrime
        {
            return null;
        }
    }
}
