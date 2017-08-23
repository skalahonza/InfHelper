namespace InfHelper.Models.Tokens
{
    public class CategoryOpeningToken : TokenBase
    {
        public override char[] Symbols { get; } = {'['};
        public override TokenType Type { get; } =TokenType.CategoryOpening;
    }
}