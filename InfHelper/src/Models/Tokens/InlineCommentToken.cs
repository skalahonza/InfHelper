namespace InfHelper.Models.Tokens
{
    public class InlineCommentToken : IToken
    {
        public char[] Symbols { get; } = {';'};
        public TokenType Type { get; } =TokenType.InlineComment;
    }
}