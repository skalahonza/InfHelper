namespace InfHelper.Models.Tokens
{
    public class WhiteSpaceToken : IToken
    {
        public char[] Symbols { get; } = { ' ', '\t' ,' '};
        public char Symbol { get; set; }
        public TokenType Type { get; } = TokenType.WhiteSpace;
    }
}