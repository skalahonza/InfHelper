namespace InfHelper.Models.Tokens
{
    public class InlineCommentToken : IToken
    {
        public string[] Symbols { get; } = {";"};
        public TokenType Type { get; } =TokenType.InlineComment;
    }
}