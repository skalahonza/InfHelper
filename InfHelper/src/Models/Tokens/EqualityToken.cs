namespace InfHelper.Models.Tokens
{
    public class EqualityToken : IToken
    {
        public char[] Symbols { get; } = {'='};
        public TokenType Type { get; } = TokenType.EQ;
    }
}