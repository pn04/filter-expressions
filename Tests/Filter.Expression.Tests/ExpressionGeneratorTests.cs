namespace Filter.Expression.Tests;

using System.Linq.Expressions;
using filter.expressions;

public class ExpressionGeneratorTests
{

    [Fact]
    public void GetCompleteExpression_WhenCalledWithAnIdentifierEqualsAndAStringValue_ReturnsEqualsExpression()
    {
        //Arrange
        var tokens = new List<Token>
        {
            new Token
            {
                Type = TokenType.Identifier,
                Value = "Name"
            },
            new Token
            {
                Type = TokenType.Equals,
                Value = "="
            },
            new Token
            {
                Type = TokenType.String,
                Value = "John"
            }
        };
        var generator = new ExpressionGenerator();
        //Act
        var result = generator.BuildExpression<Person>(tokens);
        var expected = Expression.Lambda<Func<Person, bool>>
                        (
                            Expression.Equal(Expression.Property(Expression.Parameter(typeof(Person), "p"), "Name"),
                                             Expression.Constant("John")), Expression.Parameter(typeof(Person), "p")
                        );
        //Assert
        Assert.Equal(expected.ToString(), result.ToString());
    }

    [Fact]
    public void GetCompleteExpression_WhenCalledWithAString_ReturnsEqualsExpression()
    {
        //Arrange
        var input = "Name=\"John\" OR (Age=\"20\" AND Name=\"Frank\")";
        var generator = new ExpressionGenerator();
        //Act
        var result = generator.BuildExpression<Person>(input);
        var expected = Expression.Lambda<Func<Person, bool>>
                        (
                            Expression.Or(
                                Expression.Equal(Expression.Property(Expression.Parameter(typeof(Person), "p"), "Name"),
                                                 Expression.Constant("John")),
                                Expression.And(
                                    Expression.Equal(Expression.Property(Expression.Parameter(typeof(Person), "p"), "Age"),
                                                     Expression.Constant("20")),
                                    Expression.Equal(Expression.Property(Expression.Parameter(typeof(Person), "p"), "Name"),
                                                     Expression.Constant("Frank"))
                                )
                            ), Expression.Parameter(typeof(Person), "p")
                        );
        //Assert
        Assert.Equal(expected.ToString(), result.ToString());
    }

    [Fact]
    public void GetCompiledQuery_WhenCalledWithAString_ReturnsEqualsExpression()
    {
        //Arrange
        var input = "Name = \"John\" OR (Age = \"20\" AND Name = \"Frank\")";
        var generator = new ExpressionGenerator();
        //Act
        var result = generator.GetCompiledQuery<Person>(input);
        var paramExpression = Expression.Parameter(typeof(Person), "p");
        var expected = Expression.Lambda<Func<Person, bool>>
                        (
                            Expression.Or(
                                Expression.Equal(Expression.Property(paramExpression, "Name"),
                                                 Expression.Constant("John")),
                                Expression.And(
                                    Expression.Equal(Expression.Property(paramExpression, "Age"),
                                                     Expression.Constant("20")),
                                    Expression.Equal(Expression.Property(paramExpression, "Name"),
                                                     Expression.Constant("Frank"))
                                )
                            ), paramExpression
                        ).Compile();
        //Assert
        Assert.Equal(expected.ToString(), result.ToString());
    }

    [Fact]
    public void GetCompiledQuery_WhenCalledWithAString_ReturnsCorrectFilteredList()
    {
        //Arrange
        var input = "Name = \"John\" OR (Age = \"20\" AND Name = \"Frank\")";
        var generator = new ExpressionGenerator();
        var list = new List<Person>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(new Person { Name = "John", Age = "20" });
            list.Add(new Person { Name = "Frank", Age = "20" });
            list.Add(new Person { Name = "John", Age = "30" });
            list.Add(new Person { Name = "Frank", Age = "30" });
        }
        //Act
        var result = list.AsQueryable().Where(generator.GetCompiledQuery<Person>(input));
        var expected = list.Where(p => p.Name == "John" || (p.Age == "20" && p.Name == "Frank"));
        //Assert
        Assert.Equal(expected.Count(), result.Count());
    }



}

public class Person
{
    public string Name { get; set; }
    public string Age { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}