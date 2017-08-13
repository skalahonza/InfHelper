using System;
using System.Collections.Generic;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public interface ITokenParser
    {
        IEnumerable<IToken> AllowedTokens { get; set; }
        IEnumerable<IToken> AllTokens { get;}
        IEnumerable<IToken> IgnoredTokens { get; set; }
        event EventHandler<IToken> InvalidTokenFound;
        event EventHandler<IToken> ValidTokenFound;

        void ParseFormula(string formula);
    }
}