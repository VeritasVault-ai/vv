using System;
using System.Linq.Expressions;

namespace vv.Data.Utilities
{
    /// <summary>
    /// Utility class for combining expressions
    /// </summary>
    public static class ExpressionCombiner
    {
        /// <summary>
        /// Combines two predicates with a logical AND
        /// </summary>
        public static Expression<Func<T, bool>> CombinePredicates<T>(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            // Create a parameter for the new lambda expression
            var parameter = Expression.Parameter(typeof(T));

            // Replace the parameter in both expressions
            var firstBody = ReplaceParameter(first.Body, first.Parameters[0], parameter);
            var secondBody = ReplaceParameter(second.Body, second.Parameters[0], parameter);

            // Combine the bodies with an AND expression
            var body = Expression.AndAlso(firstBody, secondBody);

            // Create and return a new lambda expression
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression ReplaceParameter(
            Expression expression,
            ParameterExpression oldParameter,
            ParameterExpression newParameter)
        {
            return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
        }

        /// <summary>
        /// Helper class to replace parameters in expressions
        /// </summary>
        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}