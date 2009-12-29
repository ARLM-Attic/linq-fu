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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqFu
{
    /// <summary>
    /// The main entry point into the LinqFu system.
    /// </summary>
    public class ExpressionBuilder : ExpressionVisitorBase
    {
        public override MemberAssignment Visit(MemberAssignment assignment)
        {
            Expression expression = this.Visit(assignment.Expression);
            var bindingReturn = Expression.Bind(assignment.Member, expression);

            return bindingReturn;
        }

        public override MemberMemberBinding Visit(MemberMemberBinding binding)
        {
            var bindings = new List<MemberBinding>(binding.Bindings.Count);
            foreach (var item in binding.Bindings)
            {
                var clone = this.Visit(item);
                bindings.Add(clone);
            }

            var bindingReturn = Expression.MemberBind(binding.Member, bindings);
            return bindingReturn;
        }

        public override MemberListBinding Visit(MemberListBinding binding)
        {
            var initializers = new List<ElementInit>(binding.Initializers.Count);
            foreach (var initializer in binding.Initializers)
            {
                var clone = this.Visit(initializer);
                initializers.Add(clone);
            }

            var bindingReturn = Expression.ListBind(binding.Member, initializers);
            return bindingReturn;
        }

        public override ElementInit Visit(ElementInit initializer)
        {
            var arguments = new List<Expression>(initializer.Arguments.Count);
            foreach (var item in initializer.Arguments)
            {
                var clone = this.Visit(item);
                arguments.Add(clone);
            }

            var initialierReturn = Expression.ElementInit(initializer.AddMethod, arguments);
            return initialierReturn;
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
        public override BinaryExpression Visit(BinaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            BinaryExpression expressionReturn;

            Expression left = this.Visit(expression.Left);
            Expression right = this.Visit(expression.Right);
            Expression conversion = this.Visit((Expression)expression.Conversion);

            if (expression.NodeType == ExpressionType.Coalesce && expression.Conversion != null)
            {
                expressionReturn = Expression.Coalesce(left, right, conversion as LambdaExpression);
            }
            else
            {
                expressionReturn = Expression.MakeBinary(expression.NodeType, left, right, expression.IsLiftedToNull, expression.Method);
            }

            if (expressionReturn == null) throw new NotSupportedException(String.Format("BinaryExpression NodeType of '{0}' is not supported", expression.NodeType));

            return expressionReturn;
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
        public override TypeBinaryExpression Visit(TypeBinaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var typeExpression = this.Visit(expression.Expression);
            return Expression.TypeIs(typeExpression, expression.TypeOperand);
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
        public override ConditionalExpression Visit(ConditionalExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression ifFalse = this.Visit(expression.IfFalse);
            Expression ifTrue = this.Visit(expression.IfTrue);
            Expression test = this.Visit(expression.Test);

            var expressionReturn = Expression.Condition(test, ifTrue, ifFalse);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ConstantExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override ConstantExpression Visit(ConstantExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var expressionReturn = Expression.Constant(expression.Value);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ConstantExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override ParameterExpression Visit(ParameterExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var expressionReturn = Expression.Parameter(expression.Type, expression.Name);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override MemberExpression Visit(MemberExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var expressionReturn = Expression.MakeMemberAccess(expression.Expression, expression.Member);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override MemberInitExpression Visit(MemberInitExpression expression)
        {
            NewExpression newExpression = this.Visit(expression.NewExpression);

            var bindings = new List<MemberBinding>(expression.Bindings.Count);
            foreach (var item in expression.Bindings)
            {
                var clone = this.Visit(item);
                bindings.Add(clone);
            }

            var expressionReturn = Expression.MemberInit(newExpression, bindings);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MethodCallExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override MethodCallExpression Visit(MethodCallExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var arguments = new List<Expression>(expression.Arguments.Count);
            foreach (var argument in expression.Arguments)
            {
                var clone = this.Visit(argument);
                arguments.Add(clone);
            }
            var expressionReturn = Expression.Call(expression.Object, expression.Method, arguments.ToArray());
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override LambdaExpression Visit(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var parameters = new List<ParameterExpression>(expression.Parameters.Count);
            foreach (var parameter in expression.Parameters)
            {
                var clone = this.Visit(parameter);
                parameters.Add(clone);
            }

            var delegateType = expression.Type;
            var expressionBody = this.Visit(expression.Body);

            var expressionReturn = Expression.Lambda(delegateType, expressionBody, parameters.ToArray());
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override ListInitExpression Visit(ListInitExpression expression)
        {
            NewExpression newExpression = this.Visit(expression.NewExpression);

            var initializers = new List<ElementInit>(expression.Initializers.Count);
            foreach (var initializer in expression.Initializers)
            {
                var clone = this.Visit(initializer);
                initializers.Add(clone);
            }

            var expressionReturn = Expression.ListInit(newExpression, initializers);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override NewExpression Visit(NewExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var arguments = new List<Expression>(expression.Arguments.Count);
            foreach (var argument in expression.Arguments)
            {
                var clone = this.Visit(argument);
                arguments.Add(clone);
            }

            NewExpression expressionReturn;
            var constructor = expression.Constructor;
            var members = expression.Members;

            if (members == null)
            {
                expressionReturn = Expression.New(constructor, arguments);
            }
            else
            {
                expressionReturn = Expression.New(constructor, arguments, members);
            }

            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override NewArrayExpression Visit(NewArrayExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var expressions = new List<Expression>(expression.Expressions.Count);
            foreach (var item in expression.Expressions)
            {
                var clone = this.Visit(item);
                expressions.Add(clone);
            }

            NewArrayExpression expressionReturn;

            if (expression.NodeType == ExpressionType.NewArrayInit)
            {
                expressionReturn = Expression.NewArrayInit(expression.Type.GetElementType(), expressions);
            }
            else
            {
                expressionReturn = Expression.NewArrayBounds(expression.Type.GetElementType(), expressions);
            }

            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        public override InvocationExpression Visit(InvocationExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var arguments = new List<Expression>(expression.Arguments.Count);
            foreach (var item in expression.Arguments)
            {
                var clone = this.Visit(item);
                arguments.Add(clone);
            }

            var invocationExpression = this.Visit(expression.Expression);
            var expressionReturn = Expression.Invoke(invocationExpression, arguments);

            return expressionReturn;
        }

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="UnaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public override UnaryExpression Visit(UnaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var innerExpression = this.Visit(expression.Operand);
            var expressionReturn = Expression.MakeUnary(expression.NodeType, innerExpression, expression.Type);
            return expressionReturn;
        }

        /// <summary>
        /// Performs a conversion of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <typeparam name="TFrom">The type to convert from.</typeparam>
        /// <typeparam name="TTo">The type to convert to.</typeparam>
        /// <param name="expression">The <see cref="UnaryExpression"/> type to perform a type conversion clone on.</param>
        /// <returns>A conversion of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        public UnaryExpression Convert<TFrom, TTo>(UnaryExpression expression) where TTo : TFrom
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var innerExpression = this.Visit(expression.Operand);
            var expressionReturn = Expression.MakeUnary(expression.NodeType, innerExpression, expression.Type);
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
        public Expression<Func<TTheta, Boolean>> Convert<TPrime, TTheta>(Expression<Func<TPrime, Boolean>> expression) // where TTheta : TPrime
        {
            return null;
        }
    }
}
