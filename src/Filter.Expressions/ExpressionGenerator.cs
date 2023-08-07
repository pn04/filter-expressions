namespace filter.expressions;

using System.Linq.Expressions;

public class ExpressionGenerator
{
    public Expression<Func<T, bool>> BuildExpression<T>(string input)
    {
        var tokens = new Parser().Parse(input);
        return BuildExpression<T>(tokens);
    }

    public Expression<Func<T, bool>> BuildExpression<T>(List<Token> tokens)
    {

        var paramExpression = Expression.Parameter(typeof(T), "p");
        var postFixTokens = ToPostFix(tokens);
        var stack = new Stack<Expression>();

        foreach (var token in postFixTokens)
        {
            if (token.Type == TokenType.Identifier)
            {
                var left = Expression.Property(paramExpression, token.Value);
                stack.Push(left);
            }
            else if (token.Type == TokenType.String)
            {
                var right = Expression.Constant(token.Value);
                stack.Push(right);
            }
            else if (token.Type == TokenType.Equals)
            {
                var right = stack.Pop();
                var left = stack.Pop();
                var body = Expression.Equal(left, right);
                //stack.Push(Expression.Lambda<Func<T, bool>>(body, Expression.Parameter(typeof(T), "p")));
                stack.Push(body);
            }
            else if (token.Type == TokenType.And)
            {
                var right = stack.Pop();
                var left = stack.Pop();
                var body = Expression.And(left, right);
                //stack.Push(Expression.Lambda<Func<T, bool>>(body, Expression.Parameter(typeof(T), "p")));
                stack.Push(body);
            }
            else if (token.Type == TokenType.Or)
            {
                var right = stack.Pop();
                var left = stack.Pop();
                var body = Expression.Or(left, right);
                //stack.Push(Expression.Lambda<Func<T, bool>>(body, Expression.Parameter(typeof(T), "p")));
                stack.Push(body);
            }
        }

        var genExpression = stack.Pop();
        var lambda = Expression.Lambda<Func<T, bool>>(genExpression, paramExpression);
        return lambda;
    }
    private List<Token> ToPostFix(List<Token> tokens)
    {

        //Create an empty stack
        var stack = new Stack<Token>();
        //Create an empty list for output
        var output = new List<Token>();
        //For each token in the infix list
        foreach (var token in tokens)
        {
            //If token is an operand, add it to the output list
            if (token.Type == TokenType.Identifier || token.Type == TokenType.String)
            {
                output.Add(token);
            }
            //If token is a left parenthesis, push it onto the stack
            else if (token.Type == TokenType.OpenParen)
            {
                stack.Push(token);
            }
            //If token is a right parenthesis
            else if (token.Type == TokenType.CloseParen)
            {
                //Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output list
                while (stack.Peek().Type != TokenType.OpenParen)
                {
                    output.Add(stack.Pop());
                }
                //Pop the left parenthesis from the stack, but not onto the output list
                stack.Pop();
            }
            //If token is an operator, then
            else if (token.Type == TokenType.Equals || token.Type == TokenType.And || token.Type == TokenType.Or)
            {
                //While there is an operator at the top of the stack with greater than or equal to precedence (low number represents higher precedence)
                while (stack.Count > 0 && stack.Peek().Type != TokenType.OpenParen &&
                        stack.Peek().Precedence <= token.Precedence)
                {
                    //pop operators from the stack onto the output list
                    output.Add(stack.Pop());
                }
                //push the current token onto the stack
                stack.Push(token);
            }
        }
        //When there are no more tokens to read
        //While there are still operator tokens in the stack
        while (stack.Count > 0)
        {
            //Pop the operator onto the output list
            output.Add(stack.Pop());
        }
        //Exit
        return output;
    }

    public Func<T, bool> GetCompiledQuery<T>(string input)
    {
        var expression = BuildExpression<T>(input);
        return expression.Compile();
    }
}
