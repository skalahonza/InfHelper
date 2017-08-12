﻿using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public class BasicTokenParser : ITokenParser
    {
        public ICollection<IToken> AllTokens { get; set; }
        public ICollection<IToken> AllowedTokens { get; set; }
        public ICollection<IToken> IgnoredTokens { get; set; }

        public event EventHandler<IToken> InvalidTokenFound;
        public event EventHandler<IToken> ValidTokenFound;

        public BasicTokenParser()
        {
            AllTokens = new HashSet<IToken>
            {
                new CategoryClosingToken(),
                new CategoryOpeningToken(),
                new EqualityToken(),
                new InlineCommentToken(),
                new NewLineToken(),
                new WhiteSpaceToken(),
                new LineConcatenatorToken(),
                new LetterToken(),
            };
        }

        public BasicTokenParser(ICollection<IToken> allowedTokens) : this()
        {
            AllowedTokens = allowedTokens;
        }

        public BasicTokenParser(ICollection<IToken> allTokens, ICollection<IToken> allowedTokens) : this(allowedTokens)
        {
            AllTokens = allTokens;
        }

        public virtual void ParseToken(string formula)
        {
            foreach (var c in formula)
            {
                bool found = false;

                //examine all known tokens
                foreach (var token in AllTokens)
                {
                    if (!token.Symbols.Contains(c)) continue;

                    //token found
                    token.Symbol = c;
                    found = true;

                    //not allowed token detected
                    if (AllowedTokens.All(x => x.Type != token.Type))
                    {
                        InvalidTokenFound?.Invoke(this, token);
                        continue;
                    }

                    //ignored token detected
                    if (IgnoredTokens.Any(x => x.Type == token.Type))
                        continue;

                    //allowed token detected
                    ValidTokenFound?.Invoke(this, token);
                }

                //token not recognized
                if (!found)
                    throw new NoneTokenRecognizedException("None token recognized in given formula: " + formula + Environment.NewLine + "Examined symbol: " + c);
            }
        }
    }
}