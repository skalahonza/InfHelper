using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;
using InfHelper.Models;
using InfHelper.Models.Tokens;

namespace InfHelper.Parsers
{
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
        }

        public void Parse(string content)
        {
            InitMainParsing();
            parser.ParseFormula(content);
            CategoryParsingComplete();
        }

        /// <summary>
        /// When category parsing is completed
        /// </summary>
        public event EventHandler<Category> CategoryDiscovered;

        /// <summary>
        /// Inits main parsing state. Skips inlines comments, white spaces and new lines and init new cateory parsing when category opening token found.
        /// </summary>
        private void InitMainParsing()
        {
            parser.ValidTokenFound -= ValidTokenFoundDuringKeyIdParsing;
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

        /// <summary>
        /// Parse only letter tokens, end parsing when closing token found.
        /// </summary>
        private void InitCategoryParsing()
        {
            parser.ValidTokenFound -= ValidTokenFoundDuringMainParsing;
            parser.ValidTokenFound -= ValidTokenFoundDuringKeyIdParsing;
            parser.ValidTokenFound += ValidTokenFoundDuringCategoryParsing;
            parser.IgnoredTokens = new HashSet<IToken>();
            parser.AllowedTokens = new HashSet<IToken>
            {
                new CategoryClosingToken(),
                new LetterToken()
            };
        }

        /// <summary>
        /// Parse key id.
        /// </summary>
        private void InitKeyIdParsing()
        {
            parser.ValidTokenFound -= ValidTokenFoundDuringCategoryParsing;
            parser.ValidTokenFound += ValidTokenFoundDuringKeyIdParsing;

            parser.AllowedTokens = new HashSet<IToken>
            {
                new InlineCommentToken(),
                new LetterToken(),
                new EqualityToken(),
            };

            parser.IgnoredTokens = new HashSet<IToken>();
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
                    InitKeyIdParsing();
                    break;
                case TokenType.Letter:
                    currentCategory.Name += token.Symbol;
                    break;
                default:
                    throw new InvalidTokenException("Invalid token found during parsing of the file: " + token.Symbol);
            }
        }

        //when parsing a token id
        private void ValidTokenFoundDuringKeyIdParsing(object sender, IToken token)
        {
            switch (token.Type)
            {
                case TokenType.EQ:
                    break;
                case TokenType.WhiteSpace:
                    break;
                case TokenType.Letter:
                    if (currentKey == null)
                    {
                        currentKey = new Key();
                    }
                    currentKey.Id += token.Symbol;
                    break;
                case TokenType.LineConcatenator:
                    break;
                case TokenType.CategoryOpening:
                    InitCategoryParsing();
                    CategoryParsingComplete();
                    break;
                case TokenType.InlineComment:
                    if (currentKey != null && currentKey.KeyValues.Any())
                    {
                        throw new InvalidTokenException("Inline comment detected, but key value expected.");
                    }
                    else
                    {
                        //TODO go to next line
                    }
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