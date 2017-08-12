using System;
using System.Collections.Generic;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public interface ITokenParser
    {
        ICollection<IToken> AllowedTokens { get; set; }
        ICollection<IToken> AllTokens { get; set; }
        ICollection<IToken> IgnoredTokens { get; set; }
        event EventHandler<IToken> InvalidTokenFound;
        event EventHandler<IToken> ValidTokenFound;

        void ParseToken(string formula);
    }
}