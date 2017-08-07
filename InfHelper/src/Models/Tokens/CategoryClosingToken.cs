namespace InfHelper.Models.Tokens
{
    public class CategoryClosingToken : IToken
    {
        public string[] Symbols { get; } = { "]" };
        public TokenType Type { get; } = TokenType.CategoryClosing;
    }
}