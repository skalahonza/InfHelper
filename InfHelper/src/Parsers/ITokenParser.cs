using System;
using System.Collections.Generic;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public interface ITokenParser
    {
        ISet<TokenBase> AllowedTokens { get; set; }
        ISet<TokenBase> AllTokens { get;}
        ISet<TokenBase> IgnoredTokens { get; set; }
        event EventHandler<TokenBase> InvalidTokenFound;
        event EventHandler<TokenBase> ValidTokenFound;

        void ParseFormula(string formula);
    }
}