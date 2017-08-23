namespace InfHelper.Models.Tokens
{
    public class ValueSeparatorToken : TokenBase
    {
        public override char[] Symbols { get; } = { ',' };
        public override TokenType Type { get; } = TokenType.ValueSeparator;
    }
}