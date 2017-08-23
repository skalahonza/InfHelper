namespace InfHelper.Models.Tokens
{
    public class LineConcatenatorToken : TokenBase
    {
        public override char[] Symbols { get; } = {'\\'};
        public override TokenType Type { get; } = TokenType.LineConcatenator;
    }
}