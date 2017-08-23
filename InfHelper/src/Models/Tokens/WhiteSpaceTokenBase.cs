namespace InfHelper.Models.Tokens
{
    public class WhiteSpaceTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = { ' ', '\t' ,' '};
        public override TokenType Type { get; } = TokenType.WhiteSpace;
        public override bool IsToken(char c)
        {
            return char.IsWhiteSpace(c);
        }
    }
}