using System.Linq.Expressions;

namespace BusinessLogicLayer.Common;

public class ExpressionParameterReplacer : ExpressionVisitor
{
    private readonly IReadOnlyDictionary<ParameterExpression, ParameterExpression> _parameterReplacements;

    public ExpressionParameterReplacer(IReadOnlyDictionary<ParameterExpression, ParameterExpression> parameterReplacements)
    {
        _parameterReplacements = parameterReplacements;
    }

    public ExpressionParameterReplacer(IEnumerable<ParameterExpression> fromParameters, IEnumerable<ParameterExpression> toParameters)
        : this(fromParameters.Zip(toParameters, (from, to) => new { From = from, To = to }).ToDictionary(x => x.From, x => x.To))
    {
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_parameterReplacements.TryGetValue(node, out var replacement))
        {
            node = replacement;
        }

        return base.VisitParameter(node);
    }
}
