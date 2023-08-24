using System.Linq.Expressions;
using filter.expressions;

namespace filter_expressions;

public class QueryService
{
    private readonly ExpressionGenerator _expressionGenerator;
    private readonly Parser _parser;

    public QueryService()
    {
        _parser = new Parser();
        _expressionGenerator = new ExpressionGenerator();
    }

    public Expression<Func<T, bool>> BuildExpression<T>(string input)
    {
        var tokens = _parser.Parse(input);
        return _expressionGenerator.BuildExpression<T>(tokens);
    }


    public IQueryable<T> Query<T>(IQueryable<T> queryable, string input)
    {
        var expression = BuildExpression<T>(input);
        return queryable.Where(expression);
    }
}