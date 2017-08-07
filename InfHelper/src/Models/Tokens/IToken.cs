namespace InfHelper.Models.Tokens
{
    public interface IToken
    {
        char[] Symbols { get; }
        TokenType Type { get; }
    }
}