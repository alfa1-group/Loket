using System.Linq.Expressions;
using Loket.Api.Client.Builders.Select;
using Microsoft.Kiota.Abstractions.Serialization;

namespace Loket.Api.Client.Builders.OrderBy;

public static class OrderByBuilder<T> where T : IParsable, new()
{
    public static IOrderedBuilder<T> OrderBy(Expression<Func<T, object?>> expression)
    {
        var propertyName = SelectBuilder<T>.GetPropertyName(expression);
        return new OrderedBuilder<T>(propertyName);
    }

    public static IOrderedBuilder<T> OrderByDescending(Expression<Func<T, object?>> expression)
    {
        var propertyName = SelectBuilder<T>.GetPropertyName(expression);
        return new OrderedBuilder<T>($"-{propertyName}");
    }
}