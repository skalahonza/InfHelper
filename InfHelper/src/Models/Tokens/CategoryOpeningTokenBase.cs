namespace InfHelper.Models.Tokens
{
    public class CategoryOpeningTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = {'['};
        public override TokenType Type { get; } =TokenType.CategoryOpening;
    }
}