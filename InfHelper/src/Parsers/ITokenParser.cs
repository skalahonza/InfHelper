using InfHelper.Models.Tokens;
using System;
using System.Collections.Generic;

namespace InfHelper.Parsers
{
    public interface ITokenParser
    {
        uint Length { get; }
        uint Position { get; }
        ISet<TokenBase> AllowedTokens { get; set; }
        ISet<TokenBase> AllTokens { get; }
        ISet<TokenBase> IgnoredTokens { get; set; }
        event EventHandler<TokenBase> InvalidTokenFound;
        event EventHandler<TokenBase> ValidTokenFound;

        void Parse(string formula);
    }
}