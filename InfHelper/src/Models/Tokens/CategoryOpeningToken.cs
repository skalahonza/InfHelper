namespace InfHelper.Models.Tokens
{
    public class CategoryOpeningToken : IToken
    {
        public string[] Symbols { get; } = {"["};
        public TokenType Type { get; } =TokenType.CategoryOpening;
    }
}