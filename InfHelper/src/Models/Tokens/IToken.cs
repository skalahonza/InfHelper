namespace InfHelper.Models.Tokens
{
    public interface IToken
    {
        string[] Symbols { get; }
        TokenType Type { get; }
    }
}