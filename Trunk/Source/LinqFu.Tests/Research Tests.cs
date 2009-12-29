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

using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqFu.Tests.Data;
using NUnit.Framework;

namespace LinqFu.Tests
{
    [TestFixture()]
    public class ExpressionBuilderTests
    {
        private static void RenderExpression(Expression expression)
        {
            ExpressionWindow.RenderExpression(expression);
        }

        [Test()]
        public void DynamicQuery()
        {
            var context = new AdventureWorksDataContext();
            var expression = (MethodCallExpression)(from p in context.Products
                                                         where p.ProductID == 1
                                                         select p).Expression;
            var et = expression.Type;
            var input = ((UnaryExpression)expression.Arguments.Last()).Operand;
            var other = Expression.Call(input, expression.Method, expression.Arguments);
            MethodInfo method = expression.Method;

            

            var query = ((IQueryable<Product>) context.Products).Provider.CreateQuery(other);
            var output1 = ((IQueryable<Product>) context.Products).Provider.Execute(expression);
            Assert.That(output1, Is.Not.Null);
            var output2 = ((IQueryable<Product>)context.Products).Provider.Execute(other);
            Assert.That(output2, Is.Not.Null);

            Assert.That(output1.GetType(), Is.EqualTo(output2.GetType()));
        }

        [Test()]
        public void CloneQuery()
        {
            var context = new AdventureWorksDataContext();
            var expressionPrime = (MethodCallExpression)(from p in context.Products
                        where p.ProductID >= 0
                        select p).Expression;
            var input = ((UnaryExpression) expressionPrime.Arguments.Last()).Operand;
            var expressionTheta = ExpressionBuilder.Clone(expressionPrime);
            RenderExpression(expressionTheta);
        }
    }
}
