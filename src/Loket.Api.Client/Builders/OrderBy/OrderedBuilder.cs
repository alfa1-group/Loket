using System.Linq.Expressions;
using System.Text;
using Loket.Api.Client.Builders.Select;
using Microsoft.Kiota.Abstractions.Serialization;

namespace Loket.Api.Client.Builders.OrderBy;

internal class OrderedBuilder<T> : IOrderedBuilder<T> where T : IParsable, new()
{
    private readonly StringBuilder _builder;

    internal OrderedBuilder(string initialOrderBy)
    {
        _builder = new StringBuilder(initialOrderBy);
    }

    public IOrderedBuilder<T> ThenBy(Expression<Func<T, object?>> expression)
    {
        AddOrderBy(expression, "asc");
        return this;
    }

    public IOrderedBuilder<T> ThenByDescending(Expression<Func<T, object?>> expression)
    {
        AddOrderBy(expression, "desc");
        return this;
    }

    private void AddOrderBy(Expression<Func<T, object?>> expression, string direction)
    {
        _builder.Append(", ");
        var propertyName = SelectBuilder<T>.GetPropertyName(expression);
        _builder.Append($"{propertyName} {direction}");
    }

    public string Build() => _builder.ToString();
}