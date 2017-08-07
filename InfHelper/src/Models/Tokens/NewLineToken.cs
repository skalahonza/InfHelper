using System;

namespace InfHelper.Models.Tokens
{
    public class NewLineToken : IToken
    {
        public string[] Symbols { get; } = { Environment.NewLine, "\n" };
        public TokenType Type { get; } = TokenType.NewLine;
    }
}