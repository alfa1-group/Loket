using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Loket.Api.Client.Extensions;

namespace Loket.Api.Client.Builders.Filter;

public class FilterBuilder<T> : ExpressionVisitor, IFilterBuilder
{
    private readonly StringBuilder _filter = new();

    public string Build() => _filter.ToString();

    public static string Build(Expression<Func<T, bool>> expression)
    {
        var builder = new FilterBuilder<T>();
        builder.Visit(expression);
        return builder.Build();
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.Name == "Equals")
        {
            _filter.Append('(');
            Visit(node.Object);
            _filter.Append(" eq ");
            Visit(node.Arguments[0]);
            _filter.Append(')');
            return node;
        }

        if (TryInvokeExpressionAndVisitAsConstant(node, out var constant))
        {
            return constant;
        }

        throw new NotSupportedException($"Method '{node.Method.Name}' could not be evaluated");
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _filter.Append('(');
        Visit(node.Left);

        _filter.Append(node.NodeType switch
        {
            ExpressionType.AndAlso => " and ",
            ExpressionType.OrElse => " or ",
            ExpressionType.Equal => " eq ",
            ExpressionType.NotEqual => " ne ",
            ExpressionType.LessThan => " lt ",
            ExpressionType.LessThanOrEqual => " le ",
            ExpressionType.GreaterThan => " gt ",
            ExpressionType.GreaterThanOrEqual => " ge ",
            _ => throw new NotSupportedException($"Operator '{node.NodeType}' not supported")
        });

        Visit(node.Right);
        _filter.Append(')');

        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression?.NodeType == ExpressionType.Parameter)
        {
            _filter.Append(node.Member.Name);
        }
        else
        {
            var value = GetMemberValue(node);
            Visit(Expression.Constant(value));
        }

        return node;
    }

    protected override Expression VisitNew(NewExpression node)
    {
        if (TryInvokeExpressionAndVisitAsConstant(node, out var constant))
        {
            return constant;
        }

        throw new NotSupportedException($"Constructor for '{node.Constructor?.DeclaringType?.Name}' could not be evaluated");
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        switch (node.Value)
        {
            case string stringValue:
                _filter.Append(stringValue.ToODataFormat());
                break;

            case bool boolValue:
                _filter.Append(boolValue.ToString().ToLower());
                break;

            case double doubleValue:
                _filter.Append(doubleValue.ToString(CultureInfo.InvariantCulture));
                break;

            case short shortValue:
                _filter.Append(shortValue.ToString(CultureInfo.InvariantCulture));
                break;

            case int intValue:
                _filter.Append(intValue.ToString(CultureInfo.InvariantCulture));
                break;

            case long longValue:
                _filter.Append(longValue.ToODataFormat());
                break;

            case Guid guidValue:
                _filter.Append(guidValue.ToODataFormat());
                break;

            case DateTimeOffset dateTimeOffsetValue:
                _filter.Append(dateTimeOffsetValue.ToODataFormat());
                break;

            case DateTime dateTimeValue:
                _filter.Append(dateTimeValue.ToODataFormat());
                break;

            case null:
                _filter.Append(StringExtensions.ODataNullLiteral);
                break;

            default:
                _filter.Append(node.Value);
                break;
        }

        return node;
    }

    private static object GetMemberValue(MemberExpression member)
    {
        var objectMember = Expression.Convert(member, typeof(object));
        var getterLambda = Expression.Lambda<Func<object>>(objectMember);
        var getter = getterLambda.Compile();

        return getter();
    }

    private bool TryInvokeExpressionAndVisitAsConstant(Expression node, [NotNullWhen(true)] out Expression? constant)
    {
        try
        {
            var result = Expression.Lambda(node).Compile().DynamicInvoke();
            var constantExpression = Expression.Constant(result, node.Type);
            constant = Visit(constantExpression);
            return true;
        }
        catch
        {
            constant = null;
            return false;
        }
    }
}