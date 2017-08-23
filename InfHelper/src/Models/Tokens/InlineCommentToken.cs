namespace InfHelper.Models.Tokens
{
    public class InlineCommentToken : TokenBase
    {
        public override char[] Symbols { get; } = { ';' };
        public override TokenType Type { get; } = TokenType.InlineComment;
    }
}