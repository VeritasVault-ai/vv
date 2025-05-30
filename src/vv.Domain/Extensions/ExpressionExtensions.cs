using System;
using System.Linq.Expressions;

namespace vv.Domain.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            // Create a parameter for the combined expression
            var parameter = Expression.Parameter(typeof(T));

            // Replace parameters in the right expression with the parameter from the left expression
            var visitor = new ParameterReplacer(right.Parameters[0], parameter);
            var rightBody = visitor.Visit(right.Body);

            // Replace parameters in the left expression with the new parameter
            visitor = new ParameterReplacer(left.Parameters[0], parameter);
            var leftBody = visitor.Visit(left.Body);

            // Combine the bodies with the AND operation
            var body = Expression.AndAlso(leftBody, rightBody);

            // Create a new lambda expression with the combined bodies
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var visitor = new ParameterReplacer(right.Parameters[0], parameter);
            var rightBody = visitor.Visit(right.Body);

            visitor = new ParameterReplacer(left.Parameters[0], parameter);
            var leftBody = visitor.Visit(left.Body);

            var body = Expression.OrElse(leftBody, rightBody);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

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
                return ReferenceEquals(node, _oldParameter) ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}