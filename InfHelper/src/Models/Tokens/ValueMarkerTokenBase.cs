namespace InfHelper.Models.Tokens
{
    public class ValueMarkerTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = { '"' };
        public override TokenType Type { get; } = TokenType.ValueMarker;
    }
}