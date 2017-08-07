namespace InfHelper.Models.Tokens
{
    public class CategoryClosingToken : IToken
    {
        public char[] Symbols { get; } = { ']' };
        public char Symbol { get; set; }
        public TokenType Type { get; } = TokenType.CategoryClosing;
    }
}