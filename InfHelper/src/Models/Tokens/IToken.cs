namespace InfHelper.Models.Tokens
{
    public interface IToken
    {
        char[] Symbols { get; }
        char Symbol { get; set; }
        TokenType Type { get; }
    }
}