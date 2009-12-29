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
    /// Represents the common behaviors of all expression tree visitors.
    /// </summary>
    public interface IExpressionVisitor
    {
        /// <summary>
        /// A dispatch method to determine which one of the overloads should be used based on the concrete type of <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/> or null if <paramref name="expression"/> is null.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> is not supported by this method.</exception>
        Expression Visit(Expression expression);

        /// <summary>
        /// A dispatch method to determine which one of the overloads should be used based on the concrete type of <paramref name="binding"/>.
        /// </summary>
        /// <param name="binding">The <see cref="MemberBinding"/> to clone.</param>
        /// <returns>A clone of the supplied <paramref name="binding"/> or null if <paramref name="binding"/> is null.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="binding"/> is not supported by this method.</exception>
        MemberBinding Visit(MemberBinding binding);

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
        MemberAssignment Visit(MemberAssignment assignment);

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
        MemberMemberBinding Visit(MemberMemberBinding binding);

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
        MemberListBinding Visit(MemberListBinding binding);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="initializer"/>.
        /// </summary>
        /// <param name="initializer">The <see cref="ElementInit"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="initializer"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="initializer"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="initializer"/> is null.</exception>
        ElementInit Visit(ElementInit initializer);

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
        BinaryExpression Visit(BinaryExpression expression);

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
        TypeBinaryExpression Visit(TypeBinaryExpression expression);

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
        ConditionalExpression Visit(ConditionalExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ConstantExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        ConstantExpression Visit(ConstantExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ParameterExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        ParameterExpression Visit(ParameterExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        MemberExpression Visit(MemberExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MemberInitExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        MemberInitExpression Visit(MemberInitExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="MethodCallExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        MethodCallExpression Visit(MethodCallExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        LambdaExpression Visit(LambdaExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="ListInitExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        ListInitExpression Visit(ListInitExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="NewExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        NewExpression Visit(NewExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="NewArrayExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        NewArrayExpression Visit(NewArrayExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="InvocationExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is null.</exception>
        InvocationExpression Visit(InvocationExpression expression);

        /// <summary>
        /// Performs a deep clone of the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="UnaryExpression"/> type to deep clone.</param>
        /// <returns>A deep clone of the supplied <paramref name="expression"/>.</returns>
        /// <exception cref="NotSupportedException">The supplied <paramref name="expression"/> or one of it's inner expression nodes, is of a type that is not supported by this method.</exception>
        UnaryExpression Visit(UnaryExpression expression);
    }
}