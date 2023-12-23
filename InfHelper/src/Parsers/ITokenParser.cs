using InfHelper.Models.Tokens;
using System;
using System.Collections.Generic;

namespace InfHelper.Parsers
{
    public interface ITokenParser
    {
        uint Length { get; }
        uint Position { get; }
        ISet<TokenType> AllowedTokenTypes { get; set; }
        ISet<TokenType> AllTokenTypes { get; }
        ISet<TokenType> IgnoredTokenTypes { get; set; }
        event EventHandler<Token> InvalidTokenFound;
        event EventHandler<Token> ValidTokenFound;

        void Parse(string formula);
    }
}