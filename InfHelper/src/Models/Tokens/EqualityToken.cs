namespace InfHelper.Models.Tokens
{
    public class EqualityToken : IToken
    {
        public string[] Symbols { get; } = {"="};
        public TokenType Type { get; } = TokenType.EQ;
    }
}