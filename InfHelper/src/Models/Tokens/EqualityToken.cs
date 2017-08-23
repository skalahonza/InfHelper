namespace InfHelper.Models.Tokens
{
    public class EqualityToken : TokenBase
    {
        public override char[] Symbols { get; } = {'='};
        public override TokenType Type { get; } = TokenType.EQ;
    }
}