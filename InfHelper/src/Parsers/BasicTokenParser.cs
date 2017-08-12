using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;
using InfHelper.Models;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
    public class BasicTokenParser : ITokenParser
    {
        private EventHandler<IToken> validTokenFound;
        private EventHandler<IToken> invalidTokenFound;

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

        public BasicTokenParser(ICollection<IToken> allTokens, ICollection<IToken> allowedTokens):this(allowedTokens)
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
                        invalidTokenFound?.Invoke(this, token);
                        continue;                        
                    }

                    //ignored token detected
                    if(IgnoredTokens.Any(x => x.Type == token.Type))
                        continue;                    

                    //allowed token detected
                    validTokenFound?.Invoke(this,token);
                }

                //token not recognized
                if (!found)
                    throw new NoneTokenRecognizedException("None token recognized in given formula: " + formula + Environment.NewLine + "Examined symbol: " + c);
            }
        }
    }

    public class ContentParser
    {
        private Category currentCategory;
        private Key currentKey;
        private readonly ITokenParser parser;

        public ContentParser():this(new BasicTokenParser())
        {
        }

        public ContentParser(ITokenParser parser)
        {
            this.parser = parser;
            parser.InvalidTokenFound += InvalidTokenFound;
            InitMainParsing();
        }

        /// <summary>
        /// When category parsing is completed
        /// </summary>
        public EventHandler<Category> CategoryDiscovered { get; set; }

        private void InitMainParsing()
        {
            parser.ValidTokenFound -= ValidTokenFoundDuringKeyParsing;
            parser.ValidTokenFound += ValidTokenFoundDuringMainParsing;

            parser.AllowedTokens = new HashSet<IToken>
            {
                new InlineCommentToken(),
                new CategoryOpeningToken(),
            };

            parser.IgnoredTokens = new HashSet<IToken>
            {
                new WhiteSpaceToken(),
                new NewLineToken()
            };
        }

        private void InitCategoryParsing()
        {
            parser.ValidTokenFound -= ValidTokenFoundDuringMainParsing;
            parser.ValidTokenFound += ValidTokenFoundDuringCategoryParsing;
            parser.IgnoredTokens = new HashSet<IToken>();
            parser.AllowedTokens = new HashSet<IToken>
            {
                new CategoryClosingToken(),
                new LetterToken()
            };
        }

        private void InitKeyValuesParsing()
        {
            parser.ValidTokenFound -= ValidTokenFoundDuringCategoryParsing;
            parser.ValidTokenFound += ValidTokenFoundDuringKeyParsing;

            parser.AllowedTokens = new HashSet<IToken>
            {                
                new CategoryOpeningToken(),
                new InlineCommentToken(),
                new LetterToken(),
                new EqualityToken(),
                new LineConcatenatorToken(),
                new NewLineToken()
            };

            parser.IgnoredTokens = new HashSet<IToken>
            {
                new WhiteSpaceToken()
            };
        }

        //Parsing top layer
        private void ValidTokenFoundDuringMainParsing(object sender, IToken token)
        {
            switch (token.Type)
            {
                case TokenType.InlineComment:
                    // TODO go to next line
                    break;
                case TokenType.CategoryOpening:
                    currentCategory = new Category();
                    InitCategoryParsing();
                    break;
                default:
                    throw new InvalidTokenException("Invalid token found during parsing of the file: " + token.Symbol);
            }
        }

        //when parsing a category
        private void ValidTokenFoundDuringCategoryParsing(object sender, IToken token)
        {
            switch (token.Type)
            {
                case TokenType.CategoryClosing:
                    InitKeyValuesParsing();
                    break;                
                case TokenType.Letter:
                    currentCategory.Name += token.Symbol;
                    break;                
                default:
                    throw new InvalidTokenException("Invalid token found during parsing of the file: " + token.Symbol);
            }
        }

        private void ValidTokenFoundDuringKeyParsing(object sender, IToken token)
        {
            switch (token.Type)
            {
                case TokenType.InlineComment:
                    break;
                case TokenType.EQ:
                    break;
                case TokenType.WhiteSpace:
                    break;
                case TokenType.Letter:
                    break;
                case TokenType.NewLine:
                    break;
                case TokenType.LineConcatenator:
                    break;
                default:
                    throw new InvalidTokenException("Invalid token found during parsing of the file: " + token.Symbol);
            }
        }

        private void InvalidTokenFound(object sender, IToken token)
        {
            throw new InvalidTokenException("Invalid token found during parsing of the file: " + token.Symbol);
        }        

        private void KeyParsingComplete()
        {
            currentCategory.Keys.Add(currentKey);
            currentKey = null;
        }

        private void CategoryParsingComplete()
        {
            CategoryDiscovered?.Invoke(this, currentCategory);
            currentCategory = null;
        }
    }

}