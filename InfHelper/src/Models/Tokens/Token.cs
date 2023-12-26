namespace InfHelper.Models.Tokens
{
    public class Token
    {
        public Token(TokenType tokenType)
        {
            Type = tokenType;
        }

        public char Symbol { get; set; }
        public TokenType Type { get; }
    }
}