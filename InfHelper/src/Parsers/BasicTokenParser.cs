using InfHelper.Exceptions;
using InfHelper.Models.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfHelper.Parsers
{
    public class BasicTokenParser : ITokenParser
    {
        private ISet<TokenType> _allTokenTypes;

        public ISet<TokenType> AllTokenTypes
        {
            get => _allTokenTypes;
            set => _allTokenTypes = PrioritizeTokenTypes(value);
        }

        private ISet<TokenType> PrioritizeTokenTypes(IEnumerable<TokenType> value)
        {
            //Sort by priority - some tokens share symbols e.g. line concentrator and letter
            return new HashSet<TokenType>(value.OrderByDescending(x => (int)x));
        }

        public uint Length { get; private set; }
        public uint Position { get; private set; }

        private ISet<TokenType> _allowedTokenTypes;
        public ISet<TokenType> AllowedTokenTypes
        {
            get => _allowedTokenTypes;
            set => _allowedTokenTypes = value ?? new HashSet<TokenType>();
        }

        private ISet<TokenType> _ignoredTokenTypes;
        public ISet<TokenType> IgnoredTokenTypes
        {
            get => _ignoredTokenTypes;
            set => _ignoredTokenTypes = value ?? new HashSet<TokenType>();
        }

        public event EventHandler<Token> InvalidTokenFound;
        public event EventHandler<Token> ValidTokenFound;

        public BasicTokenParser() : this(new HashSet<TokenType>(), new HashSet<TokenType>())
        {
            AllTokenTypes = AllAvailableTokenTypes;
        }

        public BasicTokenParser(ISet<TokenType> allowedTokenTypes, ISet<TokenType> ignoredTokenTypes)
        {
            AllTokenTypes = AllAvailableTokenTypes;
            AllowedTokenTypes = allowedTokenTypes;
            IgnoredTokenTypes = ignoredTokenTypes;
        }

        public BasicTokenParser(ISet<TokenType> allTokenTypes, ISet<TokenType> allowedTokenTypes, ISet<TokenType> ignoredTokenTypes)
        {
            AllTokenTypes = allTokenTypes;
            AllowedTokenTypes = allowedTokenTypes;
            IgnoredTokenTypes = ignoredTokenTypes;
        }

        public static ISet<TokenType> AllAvailableTokenTypes => new HashSet<TokenType>(Enum.GetValues(typeof(TokenType)).Cast<TokenType>());

        public virtual void Parse(string formula)
        {
            int row = 0, col = 0;
            string line = "";

            Length = (uint)formula.Length;
            Position = 0;

            foreach (var c in formula)
            {
                Position += 1;

                if (c == '\n')
                {
                    row++;
                    col = 0;
                    line = "";
                }
                col++;
                line += c;
                HandleToken(c, row, col, line);
            }
        }

        private void HandleToken(char c, int row, int col, string line)
        {
            // examine matching TokenTypes but skip ignored
            foreach (var tokenType in AllTokenTypes
                         .Where(IsMatchingTokenType(c))
                         .Where(tokenType => !IgnoredTokenTypes.Contains(tokenType)))
            {
                GetTokenHandler(tokenType)?.Invoke(this, TokenTypes.CreateToken(tokenType, c));
                return;
            }

            // no unignored match, so let's see if we recognize the character at all 
            if (!AllTokenTypes.Any(IsMatchingTokenType(c)))
            {
                // TokenType not recognized
                throw new NoTokenRecognizedException($"TokenType not recognized in row:{row} col:{col}" + Environment.NewLine + 
                                                     "Examined symbol: " + c + Environment.NewLine +
                                                     "Symbol number: " + Convert.ToInt16(c) + Environment.NewLine +
                                                     "Examined line: " + line);
            }
        }

        private static Func<TokenType, bool> IsMatchingTokenType(char c)
        {
            return tokenType => TokenTypes.IsToken(tokenType, c);
        }

        private EventHandler<Token> GetTokenHandler(TokenType tokenType)
        {
            return AllowedTokenTypes.Contains(tokenType) ? ValidTokenFound : InvalidTokenFound;
        }
    }
}
