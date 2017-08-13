using System;
using System.Collections.Generic;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public interface ITokenParser
    {
        ISet<IToken> AllowedTokens { get; set; }
        ISet<IToken> AllTokens { get;}
        ISet<IToken> IgnoredTokens { get; set; }
        event EventHandler<IToken> InvalidTokenFound;
        event EventHandler<IToken> ValidTokenFound;

        void ParseFormula(string formula);
    }
}