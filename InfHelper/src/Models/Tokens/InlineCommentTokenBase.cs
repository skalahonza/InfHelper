namespace InfHelper.Models.Tokens
{
    public class InlineCommentTokenBase : TokenBase
    {
        public override char[] Symbols { get; } = { ';' };
        public override TokenType Type { get; } = TokenType.InlineComment;
    }
}