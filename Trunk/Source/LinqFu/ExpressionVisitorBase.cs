using System;
using System.Linq.Expressions;

namespace LinqFu
{
    /// <summary>
    /// A base implementation of the <see cref="IExpressionVisitor"/> interface performing the required dispatches to the appropriate concrete visit methods (see <see cref="Visit(Expression)"/> and <see cref="Visit(MemberBinding)"/>.
    /// </summary>
    public abstract class ExpressionVisitorBase : IExpressionVisitor
    {
        #region IExpressionVisitor Members

        /// <summary>
        /// A dispatch method to determine which one of the overloads should be used based on the concrete type of <paramref name="expression"/>.
        /// </summary>
        /// <remarks>This implementation will perform the "correct" invocation based on the actual supplied <see cref="Expression"/>.</remarks>
        /// <param name="expression">The <see cref="Expression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/> or null if <paramref name="expression"/> is null.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> is not supported by this method.</exception>
        public virtual Expression Visit(Expression expression)
        {
            if (expression == null) return null;

            Expression expressionReturn;

            var nodeType = expression.NodeType;
            switch (nodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    expressionReturn = this.Visit((UnaryExpression)expression);
                    break;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    expressionReturn = this.Visit((BinaryExpression)expression);
                    break;
                case ExpressionType.TypeIs:
                    expressionReturn = this.Visit((TypeBinaryExpression)expression);
                    break;
                case ExpressionType.Conditional:
                    expressionReturn = this.Visit((ConditionalExpression)expression);
                    break;
                case ExpressionType.Constant:
                    expressionReturn = this.Visit((ConstantExpression)expression);
                    break;
                case ExpressionType.Parameter:
                    expressionReturn = this.Visit((ParameterExpression)expression);
                    break;
                case ExpressionType.MemberAccess:
                    expressionReturn = this.Visit((MemberExpression)expression);
                    break;
                case ExpressionType.Call:
                    expressionReturn = this.Visit((MethodCallExpression)expression);
                    break;
                case ExpressionType.Lambda:
                    expressionReturn = this.Visit((LambdaExpression)expression);
                    break;
                case ExpressionType.New:
                    expressionReturn = this.Visit((NewExpression)expression);
                    break;
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    expressionReturn = this.Visit((NewArrayExpression)expression);
                    break;
                case ExpressionType.Invoke:
                    expressionReturn = this.Visit((InvocationExpression)expression);
                    break;
                case ExpressionType.MemberInit:
                    expressionReturn = this.Visit((MemberInitExpression)expression);
                    break;
                case ExpressionType.ListInit:
                    expressionReturn = this.Visit((ListInitExpression)expression);
                    break;
                default:
                    throw new NotSupportedException(String.Format(null, "The supplied expression is not supported '{0}'", expression.GetType().FullName));
            }

            return expressionReturn;
        }

        /// <summary>
        /// A dispatch method to determine which one of the overloads should be used based on the concrete type of <paramref name="binding"/>.
        /// </summary>
        /// <remarks>This implementation will perform the "correct" invocation based on the actual supplied <see cref="MemberBinding"/>.</remarks>
        /// <param name="binding">The <see cref="MemberBinding"/> to clone.</param>
        /// <returns>A clone of the supplied <paramref name="binding"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="binding"/> is not supported by this method.</exception>
        public virtual MemberBinding Visit(MemberBinding binding)
        {
            MemberBinding bindingReturn;

            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    {
                        bindingReturn = this.Visit((MemberAssignment)binding);
                        break;
                    }
                case MemberBindingType.MemberBinding:
                    {
                        bindingReturn = this.Visit((MemberMemberBinding)binding);
                        break;
                    }
                case MemberBindingType.ListBinding:
                    {
                        bindingReturn = this.Visit((MemberListBinding)binding);
                        break;
                    }
                default:
                    throw new NotSupportedException(String.Format("Unhandled binding type '{0}'", binding.BindingType));
            }

            return bindingReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="assignment"/>.
        /// </summary>
        /// <param name="assignment">The <see cref="MemberAssignment"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="assignment"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The supplied <paramref name="assignment"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</para>
        /// <para>-OR-</para>
        /// <para>The <see cref="MemberBinding.BindingType"/> value is not supported.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="assignment"/> is null.</exception>
        public abstract MemberAssignment Visit(MemberAssignment assignment);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="binding"/>.
        /// </summary>
        /// <param name="binding">The <see cref="MemberMemberBinding"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="binding"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The supplied <paramref name="binding"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</para>
        /// <para>-OR-</para>
        /// <para>The <see cref="MemberBinding.BindingType"/> value is not supported.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="binding"/> is null.</exception>
        public abstract MemberMemberBinding Visit(MemberMemberBinding binding);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="binding"/>.
        /// </summary>
        /// <param name="binding">The <see cref="MemberListBinding"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="binding"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The supplied <paramref name="binding"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</para>
        /// <para>-OR-</para>
        /// <para>The <see cref="MemberBinding.BindingType"/> value is not supported.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="binding"/> is null.</exception>
        public abstract MemberListBinding Visit(MemberListBinding binding);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="initializer"/>.
        /// </summary>
        /// <param name="initializer">The <see cref="ElementInit"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="initializer"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="initializer"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="initializer"/> is null.</exception>
        public abstract ElementInit Visit(ElementInit initializer);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="BinaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</para>
        /// <para>-OR-</para>
        /// <para>The <see cref="Expression.NodeType"/> value is not supported.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract BinaryExpression Visit(BinaryExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="TypeBinaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</para>
        /// <para>-OR-</para>
        /// <para>The <see cref="Expression.NodeType"/> value is not supported.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract TypeBinaryExpression Visit(TypeBinaryExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ConditionalExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</para>
        /// <para>-OR-</para>
        /// <para>The <see cref="Expression.NodeType"/> value is not supported.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract ConditionalExpression Visit(ConditionalExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ConstantExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract ConstantExpression Visit(ConstantExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ParameterExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract ParameterExpression Visit(ParameterExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract MemberExpression Visit(MemberExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberInitExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract MemberInitExpression Visit(MemberInitExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MethodCallExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract MethodCallExpression Visit(MethodCallExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract LambdaExpression Visit(LambdaExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ListInitExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract ListInitExpression Visit(ListInitExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="NewExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract NewExpression Visit(NewExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="NewArrayExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract NewArrayExpression Visit(NewArrayExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="InvocationExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public abstract InvocationExpression Visit(InvocationExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="UnaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public abstract UnaryExpression Visit(UnaryExpression expression);

        #endregion
    }
}
