namespace InfHelper.Models.Tokens
{
    public class EqualityTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = {'='};
        public override TokenType Type { get; } = TokenType.EQ;
    }
}