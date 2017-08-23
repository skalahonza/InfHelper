namespace InfHelper.Models.Tokens
{
    public class ValueMarkerToken : TokenBase
    {
        public override char[] Symbols { get; } = { '"' };
        public override TokenType Type { get; } = TokenType.ValueMarker;
    }
}