namespace InfHelper.Models.Tokens;

public sealed class WhiteSpaceToken : TokenBase
{
    public override char[] Symbols { get; } = [' ', '\t' ,' '];
    public override TokenType Type { get; } = TokenType.WhiteSpace;
    public override bool IsToken(char c)
    {
        return char.IsWhiteSpace(c);
    }
}