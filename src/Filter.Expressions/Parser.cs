
namespace filter.expressions;


public class Parser
{
    public List<Token> Parse(string input)
    {
        var tokens = new List<Token>();
        var token = new Token();
        var state = State.Start;
        var i = 0;
        while (i < input.Length)
        {
            var c = input[i];
            switch (state)
            {
                case State.Start:
                    if (c == '(')
                    {
                        token.Type = TokenType.OpenParen;
                        token.Value = c.ToString();
                        tokens.Add(token);
                        token = new Token();
                    }
                    else if (c == ')')
                    {
                        token.Type = TokenType.CloseParen;
                        token.Value = c.ToString();
                        tokens.Add(token);
                        token = new Token();
                    }
                    else if (c == ' ')
                    {
                        // ignore
                    }
                    else if (c == '=')
                    {
                        token.Type = TokenType.Equals;
                        token.Value = c.ToString();
                        tokens.Add(token);
                        token = new Token();
                    }
                    else if (c == '!')
                    {
                        token.Type = TokenType.Not;
                        token.Value = c.ToString();
                        tokens.Add(token);
                        token = new Token();
                    }
                    else if (c == 'A')
                    {
                        state = State.A;
                        token.Value += c;
                    }
                    else if (c == 'O')
                    {
                        state = State.O;
                        token.Value += c;
                    }
                    else if (c == 'N')
                    {
                        state = State.N;
                        token.Value += c;
                    }
                    else if (c == '"' || c == '\'')
                    {
                        state = State.String;
                        token.Type = TokenType.String;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.A:
                    if (c == 'N')
                    {
                        state = State.AN;
                        token.Value += c;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.AN:
                    if (c == 'D')
                    {
                        state = State.AND;
                        token.Value += c;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.AND:
                    if (c == ' ')
                    {
                        token.Type = TokenType.And;
                        token
                            .Value = "AND";
                        tokens.Add(token);
                        token = new Token();
                        state = State.Start;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.O:
                    if (c == 'R')
                    {
                        state = State.OR;
                        token.Value += c;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.OR:
                    if (c == ' ')
                    {
                        token.Type = TokenType.Or;
                        token.Value = "OR";
                        tokens.Add(token);
                        token = new Token();
                        state = State.Start;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.N:
                    if (c == 'O')
                    {
                        state = State.NO;
                        token.Value += c;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.NO:
                    if (c == 'T')
                    {
                        state = State.NOT;
                        token.Value += c;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.NOT:
                    if (c == ' ')
                    {
                        token.Type = TokenType.Not;
                        token.Value = "NOT";
                        tokens.Add(token);
                        token = new Token();
                        state = State.Start;
                    }
                    else
                    {
                        state = State.Identifier;
                        token.Value += c;
                    }
                    break;
                case State.Identifier:
                    if (c == ' ')
                    {
                        token.Type = TokenType.Identifier;
                        tokens.Add(token);
                        token = new Token();
                        state = State.Start;
                    }
                    else if (c == '=')
                    {
                        token.Type = TokenType.Identifier;
                        tokens.Add(token);
                        token = new Token();
                        state = State.Start;
                        token.Type = TokenType.Equals;
                        token.Value = c.ToString();
                        tokens.Add(token);
                        token = new Token();
                    }
                    else
                    {
                        token.Value += c;
                    }
                    break;
                case State.String:
                    if (c == '"' || c == '\'')
                    {
                        token.Type = TokenType.String;
                        tokens.Add(token);
                        token = new Token();
                        state = State.Start;
                    }
                    else
                    {
                        token.Value += c;
                    }
                    break;
            }
            i++;
        }
        return tokens;
    }
}


public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
    //low numbers are higher precedence
    public int Precedence
    {
        get
        {
            return Type switch
            {
                TokenType.OpenParen or TokenType.CloseParen => 0,
                TokenType.Equals => 1,
                TokenType.Not => 2,
                TokenType.And => 3,
                TokenType.Or => 3,
                _ => -1,
            };
        }
    }
}

public enum TokenType
{
    OpenParen,
    CloseParen,
    Equals,
    Not,
    And,
    Or,
    Identifier,
    String
}

public enum State
{
    Start,
    A,
    AN,
    AND,
    O,
    OR,
    N,
    NO,
    NOT,
    Identifier,
    String
}

