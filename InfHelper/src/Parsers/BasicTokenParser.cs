using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public class BasicTokenParser : ITokenParser
    {
        private ISet<IToken> allTokens;

        public ISet<IToken> AllTokens
        {
            get => allTokens;
            private set
            {
                //Sort by priority - soe tokens share symbyols e.g. line concatenator and letter
                allTokens = new HashSet<IToken>(value.OrderByDescending(x => (int)x.Type));
            }
        }

        public ISet<IToken> AllowedTokens { get; set; }
        public ISet<IToken> IgnoredTokens { get; set; }

        public event EventHandler<IToken> InvalidTokenFound;
        public event EventHandler<IToken> ValidTokenFound;

        public BasicTokenParser() : this(new HashSet<IToken>(), new HashSet<IToken>())
        {
            AllTokens = AllAvailableTokens;
        }

        public BasicTokenParser(ISet<IToken> allowedTokens, ISet<IToken> ignoredTokens)
        {
            AllTokens = AllAvailableTokens;
            AllowedTokens = allowedTokens;
            IgnoredTokens = ignoredTokens;
        }

        public BasicTokenParser(ISet<IToken> allTokens, ISet<IToken> allowedTokens, ISet<IToken> ignoredTokens)
        {
            AllTokens = allTokens;
            AllowedTokens = allowedTokens;
            IgnoredTokens = ignoredTokens;
        }

        public static ISet<IToken> AllAvailableTokens => new HashSet<IToken>
        {
            new CategoryClosingToken(),
            new CategoryOpeningToken(),
            new EqualityToken(),
            new InlineCommentToken(),
            new NewLineToken(),
            new WhiteSpaceToken(),
            new LineConcatenatorToken(),
            new LetterToken(),
            new ValueSeparatorToken(),
            new ValueMarkerToken()
        };

        public virtual void ParseFormula(string formula)
        {
            int row = 0, col = 0;
            string line = "";

            foreach (var c in formula)
            {
                bool found = false;

                if (c == '\n')
                {
                    row++;
                    col = 0;
                    line = "";
                }
                col++;
                line += c;

                //examine all known tokens
                foreach (var token in AllTokens)
                {

                    if (!token.Symbols.Contains(c)) continue;

                    //token found
                    token.Symbol = c;
                    found = true;

                    //ignored token detected
                    if (IgnoredTokens != null && IgnoredTokens.Any(x => x.Type == token.Type))
                        continue;

                    //not allowed token detected
                    if (AllowedTokens == null || AllowedTokens.All(x => x.Type != token.Type))
                    {
                        InvalidTokenFound?.Invoke(this, token);
                        continue;
                    }

                    //allowed token detected
                    ValidTokenFound?.Invoke(this, token);
                    break;
                }

                //token not recognized
                if (!found)
                    throw new NoneTokenRecognizedException($"None token recognized in row:{row} col:{col}" + Environment.NewLine + "Examined symbol: " + c
                        + "\nExamined line: " + line);
            }
        }
    }
}