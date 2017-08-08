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
        EventHandler<IToken> InvalidTokenFound { get; set; }
        EventHandler<IToken> ValidTokenFound { get; set; }

        void ParseToken(string formula);
    }
}