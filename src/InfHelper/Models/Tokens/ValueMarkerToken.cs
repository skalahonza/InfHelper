namespace InfHelper.Models.Tokens;

public sealed class ValueMarkerToken : TokenBase
{
    public override char[] Symbols { get; } = ['"'];
    public override TokenType Type { get; } = TokenType.ValueMarker;
}