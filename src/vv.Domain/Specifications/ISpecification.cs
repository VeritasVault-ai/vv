using System;
using System.Linq.Expressions;

namespace vv.Domain.Specifications
{
    /// <summary>
    /// Interface for the specification pattern
    /// </summary>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Converts the specification to an expression
        /// </summary>
        Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Checks if an entity satisfies this specification
        /// </summary>
        bool IsSatisfiedBy(T entity);
    }
}