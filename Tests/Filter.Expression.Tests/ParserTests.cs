namespace Filter.Expression.Tests;

using filter.expressions;

public class ParserTests
{
    [Fact]
    public void Parse_WhenPassedFilterWithNOT_ReturnsTokens()
    {
        var parser = new Parser();
        var tokens = parser.Parse("NOT (\"hello world\" OR \"goodbye world\")");
        Assert.Equal(6, tokens.Count);
        Assert.Equal(TokenType.Not, tokens[0].Type);
        Assert.Equal("NOT", tokens[0].Value);
        Assert.Equal(TokenType.OpenParen, tokens[1].Type);
        Assert.Equal("(", tokens[1].Value);
        Assert.Equal(TokenType.String, tokens[2].Type);
        Assert.Equal("hello world", tokens[2].Value);
        Assert.Equal(TokenType.Or, tokens[3].Type);
        Assert.Equal("OR", tokens[3].Value);
        Assert.Equal(TokenType.String, tokens[4].Type);
        Assert.Equal("goodbye world", tokens[4].Value);
        Assert.Equal(TokenType.CloseParen, tokens[5].Type);
        Assert.Equal(")", tokens[5].Value);
    }

    [Fact]
    public void Parse_WhenPassedFilter_ReturnsTokens()
    {
        var parser = new Parser();
        var tokens = parser.Parse("(state = \"CA\" AND (city = \"San Francisco\" OR city = \"Los Angeles\")) OR (state = \"NY\" AND city = \"New York\")");
        Assert.Equal(25, tokens.Count);
        Assert.Equal(TokenType.OpenParen, tokens[0].Type);
        Assert.Equal("(", tokens[0].Value);
        Assert.Equal(TokenType.Identifier, tokens[1].Type);
        Assert.Equal("state", tokens[1].Value);
        Assert.Equal(TokenType.Equals, tokens[2].Type);
        Assert.Equal("=", tokens[2].Value);
        Assert.Equal(TokenType.String, tokens[3].Type);
        Assert.Equal("CA", tokens[3].Value);
        Assert.Equal(TokenType.And, tokens[4].Type);
        Assert.Equal("AND", tokens[4].Value);
        Assert.Equal(TokenType.OpenParen, tokens[5].Type);
        Assert.Equal("(", tokens[5].Value);
        Assert.Equal(TokenType.Identifier, tokens[6].Type);
        Assert.Equal("city", tokens[6].Value);
        Assert.Equal(TokenType.Equals, tokens[7].Type);
        Assert.Equal("=", tokens[7].Value);
        Assert.Equal(TokenType.String, tokens[8].Type);
        Assert.Equal("San Francisco", tokens[8].Value);
        Assert.Equal(TokenType.Or, tokens[9].Type);
        Assert.Equal("OR", tokens[9].Value);
        Assert.Equal(TokenType.Identifier, tokens[10].Type);
        Assert.Equal("city", tokens[10].Value);
        Assert.Equal(TokenType.Equals, tokens[11].Type);
        Assert.Equal("=", tokens[11].Value);
        Assert.Equal(TokenType.String, tokens[12].Type);
        Assert.Equal("Los Angeles", tokens[12].Value);
        Assert.Equal(TokenType.CloseParen, tokens[13].Type);
        Assert.Equal(")", tokens[13].Value);
        Assert.Equal(TokenType.CloseParen, tokens[14].Type);
        Assert.Equal(")", tokens[14].Value);
        Assert.Equal(TokenType.Or, tokens[15].Type);
        Assert.Equal("OR", tokens[15].Value);
        Assert.Equal(TokenType.OpenParen, tokens[16].Type);
        Assert.Equal("(", tokens[16].Value);
        Assert.Equal(TokenType.Identifier, tokens[17].Type);
        Assert.Equal("state", tokens[17].Value);
        Assert.Equal(TokenType.Equals, tokens[18].Type);
        Assert.Equal("=", tokens[18].Value);
        Assert.Equal(TokenType.String, tokens[19].Type);
        Assert.Equal("NY", tokens[19].Value);
        Assert.Equal(TokenType.And, tokens[20].Type);
        Assert.Equal("AND", tokens[20].Value);
        Assert.Equal(TokenType.Identifier, tokens[21].Type);
        Assert.Equal("city", tokens[21].Value);
        Assert.Equal(TokenType.Equals, tokens[22].Type);
        Assert.Equal("=", tokens[22].Value);
        Assert.Equal(TokenType.String, tokens[23].Type);
        Assert.Equal("New York", tokens[23].Value);
        Assert.Equal(TokenType.CloseParen, tokens[24].Type);
        Assert.Equal(")", tokens[24].Value);
    }
}
