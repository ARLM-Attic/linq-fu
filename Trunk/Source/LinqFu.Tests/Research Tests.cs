using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using ExpressionVisualizer;
using LinqFu.Tests.Data;
using Microsoft.VisualStudio.DebuggerVisualizers;
using NUnit.Framework;

namespace LinqFu.Tests
{
    [TestFixture()]
    public class ExpressionBuilderTests : Form
    {
        private void RenderExpression(Expression expression)
        {
            VisualizerDevelopmentHost host = new VisualizerDevelopmentHost(expression,
                                                 typeof(ExpressionTreeVisualizer),
                                                 typeof(ExpressionTreeVisualizerObjectSource));
            host.ShowVisualizer(this);
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
            this.RenderExpression(expressionTheta);
        }
    }
}
