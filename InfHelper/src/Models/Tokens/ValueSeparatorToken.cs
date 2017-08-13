namespace InfHelper.Models.Tokens
{
    public class ValueSeparatorToken : IToken
    {
        public char[] Symbols { get; } = { ',' };
        public char Symbol { get; set; }
        public TokenType Type { get; } = TokenType.ValueSeparator;
    }
}