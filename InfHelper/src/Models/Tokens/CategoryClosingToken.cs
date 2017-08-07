namespace InfHelper.Models.Tokens
{
    public class CategoryClosingToken : IToken
    {
        public char[] Symbols { get; } = { ']' };
        public TokenType Type { get; } = TokenType.CategoryClosing;
    }
}