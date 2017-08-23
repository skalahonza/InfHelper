namespace InfHelper.Models.Tokens
{
    public class LineConcatenatorTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = {'\\'};
        public override TokenType Type { get; } = TokenType.LineConcatenator;
    }
}