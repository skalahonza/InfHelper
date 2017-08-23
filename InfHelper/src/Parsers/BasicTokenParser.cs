using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public class BasicTokenParser : ITokenParser
    {
        private ISet<TokenBase> allTokens;

        public ISet<TokenBase> AllTokens
        {
            get => allTokens;
            private set
            {
                //Sort by priority - soe tokens share symbyols e.g. line concatenator and letter
                allTokens = new HashSet<TokenBase>(value.OrderByDescending(x => (int)x.Type));
            }
        }

        public ISet<TokenBase> AllowedTokens { get; set; }
        public ISet<TokenBase> IgnoredTokens { get; set; }

        public event EventHandler<TokenBase> InvalidTokenFound;
        public event EventHandler<TokenBase> ValidTokenFound;

        public BasicTokenParser() : this(new HashSet<TokenBase>(), new HashSet<TokenBase>())
        {
            AllTokens = AllAvailableTokens;
        }

        public BasicTokenParser(ISet<TokenBase> allowedTokens, ISet<TokenBase> ignoredTokens)
        {
            AllTokens = AllAvailableTokens;
            AllowedTokens = allowedTokens;
            IgnoredTokens = ignoredTokens;
        }

        public BasicTokenParser(ISet<TokenBase> allTokens, ISet<TokenBase> allowedTokens, ISet<TokenBase> ignoredTokens)
        {
            AllTokens = allTokens;
            AllowedTokens = allowedTokens;
            IgnoredTokens = ignoredTokens;
        }

        public static ISet<TokenBase> AllAvailableTokens => new HashSet<TokenBase>
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

                    if (!token.IsToken(c)) continue;

                    //tokenBase found
                    token.Symbol = c;
                    found = true;

                    //ignored tokenBase detected
                    if (IgnoredTokens != null && IgnoredTokens.Any(x => x.Type == token.Type))
                        continue;

                    //not allowed tokenBase detected
                    if (AllowedTokens == null || AllowedTokens.All(x => x.Type != token.Type))
                    {
                        InvalidTokenFound?.Invoke(this, token);
                        continue;
                    }

                    //allowed tokenBase detected
                    ValidTokenFound?.Invoke(this, token);
                    break;
                }

                //tokenBase not recognized
                if (!found)
                    throw new NoneTokenRecognizedException($"None tokenBase recognized in row:{row} col:{col}" + Environment.NewLine + "Examined symbol: " + c
                        + "\nSymbol number: " + Convert.ToInt16(c)
                        + "\nExamined line: " + line);
            }
        }
    }
}