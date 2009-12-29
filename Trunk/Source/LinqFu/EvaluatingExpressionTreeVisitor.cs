#region Legal

/*
Copyright (c) 2009, James Zimmerman
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the <organization> nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL James Zimmerman BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#endregion

using System;
using System.Linq.Expressions;

namespace LinqFu
{
    /// <summary>
    /// An <see cref="IExpressionVisitor"/> that performs an evaluation of each visited <see cref="Expression"/> node against a supplied delegate.
    /// </summary>
    public class EvaluatingExpressionTreeVisitor : IExpressionVisitor
    {
        #region IExpressionVisitor Members

        /// <summary>
        /// A dispatch method to determine which one of the overloads should be used based on the concrete type of <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/> or null if <paramref name="expression"/> is null.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> is not supported by this method.</exception>
        public Expression Visit(Expression expression)
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
        /// <param name="binding">The <see cref="MemberBinding"/> to clone.</param>
        /// <returns>A clone of the supplied <paramref name="binding"/> or null if <paramref name="binding"/> is null.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="binding"/> is not supported by this method.</exception>
        public MemberBinding Visit(MemberBinding binding)
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
        public MemberAssignment Visit(MemberAssignment assignment)
        {
            throw new NotImplementedException();
        }

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
        public MemberMemberBinding Visit(MemberMemberBinding binding)
        {
            throw new NotImplementedException();
        }

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
        public MemberListBinding Visit(MemberListBinding binding)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="initializer"/>.
        /// </summary>
        /// <param name="initializer">The <see cref="ElementInit"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="initializer"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="initializer"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="initializer"/> is null.</exception>
        public ElementInit Visit(ElementInit initializer)
        {
            throw new NotImplementedException();
        }

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
        public BinaryExpression Visit(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }

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
        public TypeBinaryExpression Visit(TypeBinaryExpression expression)
        {
            throw new NotImplementedException();
        }

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
        public ConditionalExpression Visit(ConditionalExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ConstantExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public ConstantExpression Visit(ConstantExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ParameterExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public ParameterExpression Visit(ParameterExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public MemberExpression Visit(MemberExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberInitExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public MemberInitExpression Visit(MemberInitExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MethodCallExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public MethodCallExpression Visit(MethodCallExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public LambdaExpression Visit(LambdaExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ListInitExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public ListInitExpression Visit(ListInitExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="NewExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public NewExpression Visit(NewExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="NewArrayExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public NewArrayExpression Visit(NewArrayExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="InvocationExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public InvocationExpression Visit(InvocationExpression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="UnaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public UnaryExpression Visit(UnaryExpression expression)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
