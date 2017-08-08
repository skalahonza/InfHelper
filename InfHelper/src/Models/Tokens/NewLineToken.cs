namespace InfHelper.Models.Tokens
{
    public class NewLineToken : IToken
    {
        public char[] Symbols { get; } = { '\n' };
        public char Symbol { get; set; }
        public TokenType Type { get; } = TokenType.NewLine;
    }
}