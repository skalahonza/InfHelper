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
        private KeyValue curentValue;
        private string keyTmpValue;

        public ContentParser() : this(new BasicTokenParser())
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
        protected void InitMainParsing()
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
        protected void InitCategoryParsing()
        {
            currentCategory = new Category();
            parser.ValidTokenFound -= ValidTokenFoundDuringMainParsing;
            parser.ValidTokenFound -= ValidTokenFoundDuringKeyIdParsing;
            parser.ValidTokenFound += ValidTokenFoundDuringCategoryParsing;
            parser.IgnoredTokens?.Clear();
            parser.AllowedTokens = new HashSet<IToken>
            {
                new CategoryClosingToken(),
                new LetterToken()
            };
        }

        /// <summary>
        /// Parse key id.
        /// </summary>
        protected void InitKeyIdParsing()
        {
            currentKey = new Key();
            parser.ValidTokenFound -= ValidTokenFoundDuringCategoryParsing;
            parser.ValidTokenFound += ValidTokenFoundDuringKeyIdParsing;

            parser.AllowedTokens = new HashSet<IToken>
            {
                new InlineCommentToken(),
                new LetterToken(),
                new EqualityToken(),
                new WhiteSpaceToken(),
                new CategoryOpeningToken(),
                new NewLineToken(),
            };

            parser.IgnoredTokens = new HashSet<IToken>();
        }

        /// <summary>
        /// Parse values for current key
        /// </summary>
        protected void InitKeyValueParsing()
        {
            //TODO IMPLEMENT THIS
        }

        //Parsing top layer
        protected void ValidTokenFoundDuringMainParsing(object sender, IToken token)
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
        protected void ValidTokenFoundDuringCategoryParsing(object sender, IToken token)
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
        protected void ValidTokenFoundDuringKeyIdParsing(object sender, IToken token)
        {
            switch (token.Type)
            {
                case TokenType.NewLine:
                    SerializeCurrentTmpValueAsAnonymousKey();
                    break;
                case TokenType.EQ:
                    // multiple EQ tokens in formula
                    if (!string.IsNullOrEmpty(currentKey.Id))
                    {
                        throw new InvalidTokenException("Equality token detected, but not expected.");
                    }
                    currentKey.Id = keyTmpValue;                    
                    InitKeyValueParsing();
                    break;
                case TokenType.WhiteSpace:
                    //ignore spaces at the begining
                    if (!string.IsNullOrEmpty(keyTmpValue))
                    {
                        keyTmpValue += token.Symbol;
                    }
                    break;
                case TokenType.Letter:
                    keyTmpValue += token.Symbol;
                    break;
                case TokenType.CategoryOpening:
                    KeyParsingComplete();
                    CategoryParsingComplete();
                    InitCategoryParsing();
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

        protected void InvalidTokenFound(object sender, IToken token)
        {
            throw new InvalidTokenException("Invalid token found during parsing of the file: " + token.Symbol);
        }

        protected void KeyParsingComplete()
        {
            if (currentKey != null)
            {
                currentCategory.Keys.Add(currentKey);
                currentKey = null;
            }
        }

        protected void ValueParsingComplete()
        {
            if (curentValue != null)
            {
                currentKey.KeyValues.Add(curentValue);
                curentValue = null;
            }
        }

        protected void CategoryParsingComplete()
        {
            if (currentCategory != null)
            {
                CategoryDiscovered?.Invoke(this, currentCategory);
                currentCategory = null;
            }
        }

        protected void SerializeCurrentTmpValueAsAnonymousKey()
        {
            if (!string.IsNullOrEmpty(keyTmpValue))
            {
                currentKey.KeyValues = KeyValues.GetKeyValuesFrom(keyTmpValue);
                keyTmpValue = null;
                KeyParsingComplete();
            }
        }
    }
}