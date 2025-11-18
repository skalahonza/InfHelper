namespace InfHelper.Models.Tokens;

public sealed class CategoryClosingToken : TokenBase
{
    public override char[] Symbols { get; } = [']'];
    
    public override TokenType Type { get; } = TokenType.CategoryClosing;
}