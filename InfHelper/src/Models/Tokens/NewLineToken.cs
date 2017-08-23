namespace InfHelper.Models.Tokens
{
    public class NewLineToken : TokenBase
    {
        public override char[] Symbols { get; } = { '\n','\r' };
        public override TokenType Type { get; } = TokenType.NewLine;
    }
}