namespace InfHelper.Models.Tokens
{
    public class SpaceToken : TokenBase
    {
        public override char[] Symbols { get; } = { ' ' };
        public override TokenType Type { get; } = TokenType.Space;
    }
}