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
        /// <summary>
        /// A dispatch method to determine which one of the overloads should be used based on the concrete type of <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> is not supported by this method.</exception>
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
            else if (expressionType == typeof(ParameterExpression))
            {
                expressionReturn = expression;
            }
            else if (expressionType == typeof(MemberExpression))
            {
                expressionReturn = Clone((MemberExpression) expression);
            }
            else if (expressionType.IsGenericType && expressionType.GetGenericTypeDefinition() == typeof(Expression<>))
            {
                expressionReturn = Clone((LambdaExpression)expression);
            }

            if (expressionReturn == null) throw new NotSupportedException(String.Format(null, "The supplied expression is not supported '{0}'", expression.GetType().FullName));
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="UnaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public static UnaryExpression Clone(UnaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var innerExpression = Clone(expression.Operand);
            var expressionReturn = Expression.MakeUnary(expression.NodeType, innerExpression, expression.Type);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ConstantExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public static ConstantExpression Clone(ConstantExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var expressionReturn = Expression.Constant(expression.Value);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MethodCallExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
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

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="BinaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method</para>
        /// <para>-OR-</para>
        /// <para>The <see cref="Expression.NodeType"/> value is not supported.</para>
        /// </exception>
        public static BinaryExpression Clone(BinaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            BinaryExpression expressionReturn;

            Expression left = Clone(expression.Left);
            Expression right = Clone(expression.Right);
            var method = expression.Method;

            // Some temporary code here. This will all go away in Vistor Land when we refactor
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    {
                        expressionReturn = Expression.Add(left, right, method);
                        break;
                    }
                case ExpressionType.And:
                    {
                        expressionReturn = Expression.And(left, right, method);
                        break;
                    }
                case ExpressionType.AndAlso:
                    {
                        expressionReturn = Expression.And(left, right, method);
                        break;
                    }
                case ExpressionType.AddChecked:
                    {
                        expressionReturn = Expression.AddChecked(left, right, method);
                        break;
                    }
                case ExpressionType.Divide:
                    {
                        expressionReturn = Expression.Divide(left, right, method);
                        break;
                    }
                case ExpressionType.Equal:
                    {
                        expressionReturn = Expression.Equal(left, right, expression.IsLiftedToNull, method);
                        break;
                    }
                case ExpressionType.GreaterThan:
                    {
                        expressionReturn = Expression.GreaterThan(left, right, expression.IsLiftedToNull, method);
                        break;
                    }
                case ExpressionType.GreaterThanOrEqual:
                    {
                        expressionReturn = Expression.GreaterThanOrEqual(left, right, expression.IsLiftedToNull, method);
                        break;
                    }
                case ExpressionType.LessThan:
                    {
                        expressionReturn = Expression.LessThan(left, right, expression.IsLiftedToNull, method);
                        break;
                    }
                case ExpressionType.LessThanOrEqual:
                    {
                        expressionReturn = Expression.LessThanOrEqual(left, right, expression.IsLiftedToNull, method);
                        break;
                    }
                case ExpressionType.Modulo:
                    {
                        expressionReturn = Expression.Modulo(left, right, method);
                        break;
                    }
                case ExpressionType.Multiply:
                    {
                        expressionReturn = Expression.Multiply(left, right, method);
                        break;
                    }
                case ExpressionType.MultiplyChecked:
                    {
                        expressionReturn = Expression.MultiplyChecked(left, right, method);
                        break;
                    }
                case ExpressionType.NotEqual:
                    {
                        expressionReturn = Expression.NotEqual(left, right, expression.IsLiftedToNull, method);
                        break;
                    }
                case ExpressionType.Or:
                    {
                        expressionReturn = Expression.Or(left, right, method);
                        break;
                    }
                case ExpressionType.OrElse:
                    {
                        expressionReturn = Expression.OrElse(left, right, method);
                        break;
                    }
                case ExpressionType.Power:
                    {
                        expressionReturn = Expression.Power(left, right, method);
                        break;
                    }
                default: throw new NotSupportedException(String.Format("BinaryExpression NodeType of '{0}' is not supported", expression.NodeType));
            }
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public static LambdaExpression Clone(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var parameters = new List<ParameterExpression>(expression.Parameters.Count);
            foreach (var parameter in expression.Parameters)
            {
                var clone = (ParameterExpression)Clone(parameter);
                parameters.Add(clone);
            }

            var delegateType = expression.Type;
            var expressionBody = Clone(expression.Body);

            var expressionReturn = Expression.Lambda(delegateType, expressionBody, parameters.ToArray());
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public static MemberExpression Clone(MemberExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var expressionReturn = Expression.MakeMemberAccess(expression.Expression, expression.Member);
            return expressionReturn;
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
