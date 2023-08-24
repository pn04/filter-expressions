using filter_expressions;

namespace Filter.Expression.Tests;

//integration tests
public class QueryServiceTests
{
    [Fact]
    public void QueryService_Where()
    {
        var queryable = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Doe" },
            new Person { FirstName = "John", LastName = "Smith" },
            new Person { FirstName = "Jane", LastName = "Smith" },
        }.AsQueryable();

        var queryService = new QueryService();
        var result = queryService.Query(queryable, "FirstName=\"John\"");

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void QueryService_Where_With_And()
    {
        var queryable = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Doe" },
            new Person { FirstName = "John", LastName = "Smith" },
            new Person { FirstName = "Jane", LastName = "Smith" },
        }.AsQueryable();

        var queryService = new QueryService();
        var result = queryService.Query(queryable, "FirstName=\"John\" AND LastName=\"Doe\"");

        Assert.Single(result);
    }

    [Fact]
    public void QueryService_Where_With_Or()
    {
        var queryable = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Doe" },
            new Person { FirstName = "John", LastName = "Smith" },
            new Person { FirstName = "Jane", LastName = "Smith" },
        }.AsQueryable();

        var queryService = new QueryService();
        var result = queryService.Query(queryable, "FirstName=\"John\" OR LastName=\"Doe\"");

        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void QueryService_Where_With_Or_And()
    {
        var queryable = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Doe" },
            new Person { FirstName = "John", LastName = "Smith" },
            new Person { FirstName = "Jane", LastName = "Smith" },
        }.AsQueryable();

        var queryService = new QueryService();
        var result = queryService.Query(queryable, "FirstName=\"John\" OR (LastName=\"Doe\" AND FirstName=\"Jane\"");

        Assert.Equal(3, result.Count());

    }

}
