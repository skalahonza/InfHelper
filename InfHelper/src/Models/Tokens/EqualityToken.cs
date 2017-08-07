namespace InfHelper.Models.Tokens
{
    public class EqualityToken : IToken
    {
        public char[] Symbols { get; } = {'='};
        public char Symbol { get; set; }
        public TokenType Type { get; } = TokenType.EQ;
    }
}