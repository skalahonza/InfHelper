using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public class TokenParser
    {
        private readonly ICollection<IToken> AllTokens;
        private readonly ICollection<IToken> AllowedTokens;

        public EventHandler<IToken> InvalidTokenFound { get; set; }

        public TokenParser()
        {
            AllTokens = new List<IToken>
            {
                new CategoryClosingToken(),
                new CategoryOpeningToken(),
                new EqualityToken(),
                new InlineCommentToken(),
            };
        }

        public TokenParser(ICollection<IToken> allowedTokens) : this()
        {
            AllowedTokens = allowedTokens;
        }

        public void ParseToken(string formula)
        {
            foreach (var c in formula)
            {
                bool found = false;

                //examine all known tokens
                foreach (var token in AllTokens)
                {
                    //examine token
                    if (!token.Symbols.Contains(c)) continue;
                    token.Symbol = c;
                    found = true;

                    //not allowed token
                    if (AllowedTokens.All(x => x.Type != token.Type))
                        InvalidTokenFound?.Invoke(this, token);
                }

                //token not recognized
                if (!found)
                    throw new NoneTokenRecognizedException("None token recognized in given formula: " + formula);
            }
        }
    }
}