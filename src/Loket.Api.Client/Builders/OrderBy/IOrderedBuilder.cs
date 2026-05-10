using System.Linq.Expressions;
using Microsoft.Kiota.Abstractions.Serialization;

namespace Loket.Api.Client.Builders.OrderBy;

public interface IOrderedBuilder<T> where T : IParsable, new()
{
    IOrderedBuilder<T> ThenBy(Expression<Func<T, object?>> expression);

    IOrderedBuilder<T> ThenByDescending(Expression<Func<T, object?>> expression);

    string Build();
}