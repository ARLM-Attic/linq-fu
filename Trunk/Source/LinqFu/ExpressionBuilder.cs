using System;
using System.Linq.Expressions;

namespace LinqFu
{
    /// <summary>
    /// The main entry point into the LinqFu system.
    /// </summary>
    public class ExpressionBuilder
    {
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
            expression.
            return null;
        }
    }
}
