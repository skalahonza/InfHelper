namespace InfHelper.Models.Tokens
{
    public class EndToken : IToken
    {
        public string[] Symbols { get; } = { };
        public TokenType Type { get; } = TokenType.END;
    }
}