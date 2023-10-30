using System.Linq.Expressions;
using Velocity.Shared.Contracts;

namespace Velocity.Backend.Specifications;

public interface ISpecification<T> where T : class, IEntity
{
    Expression<Func<T, bool>> FilterCondition { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
}

public class BaseSpecification<T> : ISpecification<T> where T : class, IEntity
{
    public Expression<Func<T, bool>> FilterCondition { get; set; }

    public List<Expression<Func<T, object>>> Includes { get; } = new();

    public List<string> IncludeStrings { get; } = new();

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
}