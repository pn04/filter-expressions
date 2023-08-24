//write an extension method for IQueryable<T> that takes a string and returns IQueryable<T> 
//that has the expression applied to it

namespace filter_expressions;

public static class QueryableExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, string input)
    {
        var queryService = new QueryService();
        return queryService.Query(queryable, input);
    }
}