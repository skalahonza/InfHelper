namespace InfHelper.Models.Tokens
{
    public class ValueSeparatorTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = { ',' };
        public override TokenType Type { get; } = TokenType.ValueSeparator;
    }
}