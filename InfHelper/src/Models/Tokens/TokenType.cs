using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfHelper.Models.Tokens
{

    internal abstract class PredicateBasedTokenTypeAttribute: Attribute
    {
        protected PredicateBasedTokenTypeAttribute(Predicate<char> predicate)
        {
            Predicate = predicate;
        }

        internal readonly Predicate<char> Predicate;

    }
    
    internal class SymbolBasedTokenTypeAttribute: Attribute
    {
        internal SymbolBasedTokenTypeAttribute(string symbols)
        {
            Symbols = symbols.ToCharArray();
        }

        internal SymbolBasedTokenTypeAttribute(char symbol)
        {
            Symbols = new []{symbol};
        }

        public char[] Symbols { get; }
    }
    
    internal class LetterTokenTypeAttribute: PredicateBasedTokenTypeAttribute
    {
        public LetterTokenTypeAttribute() : base(c => !char.IsControl(c) && !char.IsWhiteSpace(c))
        {
        }
    }

    internal class WhiteSpaceTokenTypeAttribute: PredicateBasedTokenTypeAttribute
    {
        public WhiteSpaceTokenTypeAttribute() : base(char.IsWhiteSpace)
        {
        }

    }

    public enum TokenType
    {
        [LetterTokenType] Letter,
        [SymbolBasedTokenType('=')] Equality,
        [SymbolBasedTokenType('[')] CategoryOpening,
        [SymbolBasedTokenType(']')] CategoryClosing,
        [WhiteSpaceTokenType] WhiteSpace,        
        [SymbolBasedTokenType("\n\r")] NewLine,
        [SymbolBasedTokenType('\\')] LineConcatenator,
        [SymbolBasedTokenType(',')] ValueSeparator,
        [SymbolBasedTokenType('"')] ValueMarker,
        [SymbolBasedTokenType(';')] InlineComment,
        [SymbolBasedTokenType(' ')] Space
    }
    
    public static class TokenTypes
    {

        private static readonly Dictionary<TokenType, Predicate<char>> Predicates = Initialize();

        private static Dictionary<TokenType, Predicate<char>> Initialize()
        {
            var predicates = new Dictionary<TokenType, Predicate<char>>();
            foreach (var tokenType in Enum.GetValues(typeof(TokenType)).Cast<TokenType>())
            {
                switch (GetAttr(tokenType))
                {
                    case PredicateBasedTokenTypeAttribute attribute:
                        predicates.Add(tokenType, attribute.Predicate);
                        break;
                    case SymbolBasedTokenTypeAttribute attribute:
                        predicates.Add(tokenType, c => attribute.Symbols.Contains(c));
                        break;
                    default:
                        throw new Exception("Misconfigured TokenType: " + tokenType);
                }
            } 
            return predicates;
        }

        public static bool IsToken(TokenType tokenType, char c)
        {
            return Predicates[tokenType].Invoke(c);
        }
        
        public static Token CreateToken(TokenType tokenType, char c)
        {
            return new Token(tokenType)
            {
                Symbol = c
            };
        }

        private static Attribute GetAttr(TokenType tokenType)
        {
            var memberInfo = ForValue(tokenType);
            if (memberInfo == null)
            {
                throw new Exception("Misconfigured TokenType: " + tokenType);
            }

            return Attribute.GetCustomAttribute(memberInfo, typeof(Attribute));
        }

        private static MemberInfo ForValue(TokenType tokenType)
        {
            var name = Enum.GetName(typeof(TokenType), tokenType);
            return name != null ? typeof(TokenType).GetField(name) : null;
        }

    }
    
}