namespace InfHelper.Models.Tokens
{
    public class CategoryOpeningToken : IToken
    {
        public char[] Symbols { get; } = {'['};
        public TokenType Type { get; } =TokenType.CategoryOpening;
    }
}