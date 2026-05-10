using System.Linq.Expressions;
using System.Reflection;

namespace Loket.Api.Client.Builders.Select;

internal static class SelectBuilder<T>
{
    private static readonly Dictionary<string, string>? PropertyMapping = (typeof(T).GetField("PropertyMapping", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as Dictionary<string, string>)?
        .Where(p => p.Key != "Metadata")
        .ToDictionary(pair => pair.Key, pair => pair.Value);

    /// <summary>
    /// Creates a CSV string of property names from the provided lambda expressions. When no expressions are provided, it returns all instance public properties with a getter.
    /// </summary>
    /// <typeparam name="T">The type to extract property names from</typeparam>
    /// <param name="expressions">Lambda expressions pointing to properties</param>
    /// <returns>A comma-separated string of property names</returns>
    internal static string Build(params Expression<Func<T, object?>>[] expressions)
    {
        if (expressions.Length == 0)
        {
            return string.Join(", ", PropertyMapping?.Select(f => f.Value) ?? []);
        }

        var propertyNames = new List<string>();
        foreach (var expression in expressions)
        {
            var propertyName = GetPropertyName(expression);
            if (!string.IsNullOrEmpty(propertyName))
            {
                propertyNames.Add(propertyName);
            }
        }

        return string.Join(", ", propertyNames);
    }

    /// <summary>
    /// Creates a CSV string of property names from an anonymous object expression.
    /// </summary>
    /// <typeparam name="T">The source type</typeparam>
    /// <param name="expression">Lambda expression that returns an anonymous object with the desired properties</param>
    /// <returns>A comma-separated string of property names</returns>
    internal static string Build(Expression<Func<T, object?>> expression)
    {
        return expression.Body switch
        {
            NewExpression newExpression => ExtractFromNewExpression(newExpression),
            MemberExpression memberExpression => memberExpression.Member.Name,
            UnaryExpression { Operand: MemberExpression memberExpr } => memberExpr.Member.Name,
            _ => throw new ArgumentException($"Expression must be a property access or anonymous object constructor. Found {expression.Body.GetType().Name} instead. Examples: x => x.PropertyName or x => new {{ x.Prop1, x.Prop2 }}", nameof(expression))
        };
    }

    private static string ExtractFromNewExpression(NewExpression newExpression)
    {
        var propertyNames = new List<string>();

        foreach (var argExpr in newExpression.Arguments)
        {
            var propertyName = argExpr switch
            {
                MemberExpression memberExpr => memberExpr.Member.Name,
                UnaryExpression { Operand: MemberExpression unaryMemberExpr } => unaryMemberExpr.Member.Name,
                _ => throw new ArgumentException($"Invalid expression in anonymous object: {argExpr.GetType().Name}")
            };

            propertyNames.Add(propertyName);
        }

        return string.Join(", ", propertyNames);
    }

    internal static string GetPropertyName(Expression<Func<T, object?>> expression)
    {
        var name = expression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member.Name,
            UnaryExpression { Operand: MemberExpression memberExpr } => memberExpr.Member.Name,
            _ => throw new ArgumentException($"Expression must be a property access. Found {expression.Body.GetType().Name} instead. Example: x => x.PropertyName", nameof(expression))
        };

        if (PropertyMapping?.TryGetValue(name, out var mappedName) == true)
        {
            return mappedName;
        }

        return name;
    }
}