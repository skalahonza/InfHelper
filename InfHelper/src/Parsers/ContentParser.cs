using InfHelper.Exceptions;
using InfHelper.Models;
using InfHelper.Models.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfHelper.Parsers
{
    public class ContentParser
    {
        private Category _currentCategory;
        private Key _currentKey;
        private readonly ITokenParser _parser;
        private string _keyTmpValue;
        private Action _previousParsing;
        private string _parsingType = "";

        public ContentParser() : this(new BasicTokenParser())
        {

        }

        public ContentParser(ITokenParser parser)
        {
            _parser = parser;
            _parser.InvalidTokenFound += InvalidTokenFound;
        }

        public void Parse(string content)
        {
            InitMainParsing();
            _parser.Parse(content);
            ValueParsingComplete();
            KeyParsingComplete();
            CategoryParsingComplete();
        }

        /// <summary>
        /// When category parsing is completed
        /// </summary>
        public event EventHandler<Category> CategoryDiscovered;

        /// <summary>
        /// Inits main parsing state. Skips inlines comments, white spaces and new lines and init new category parsing when category opening tokenBase found.
        /// </summary>
        protected void InitMainParsing()
        {
            _parsingType = "main parsing";
            ClearAllMyCallbacks();
            _parser.ValidTokenFound += ValidTokenFoundDuringMainParsing;

            _parser.AllowedTokenTypes = new HashSet<TokenType>
            {
                TokenType.InlineComment,
                TokenType.CategoryOpening,
            };

            _parser.IgnoredTokenTypes = new HashSet<TokenType>
            {
                TokenType.WhiteSpace,
                TokenType.NewLine
            };
        }

        /// <summary>
        /// Parse only letter tokens, end parsing when closing tokenBase found.
        /// </summary>
        protected void InitCategoryParsing()
        {
            _parsingType = "category parsing";
            _currentCategory = new Category();
            ClearAllMyCallbacks();
            _parser.ValidTokenFound += ValidTokenFoundDuringCategoryParsing;
            _parser.IgnoredTokenTypes?.Clear();
            _parser.AllowedTokenTypes = new HashSet<TokenType>
            {
                TokenType.Space,
                TokenType.CategoryClosing,
                TokenType.Letter,
                TokenType.LineConcatenator
            };
        }

        /// <summary>
        /// Parse key id.
        /// </summary>
        protected void InitKeyIdParsing()
        {
            _parsingType = "key id parsing";
            _currentKey = new Key();
            ClearAllMyCallbacks();
            _parser.ValidTokenFound += ValidTokenFoundDuringKeyIdParsing;

            _parser.AllowedTokenTypes = new HashSet<TokenType>
            {
                TokenType.InlineComment,
                TokenType.Letter,
                TokenType.Equality,
                TokenType.Space,
                TokenType.WhiteSpace,
                TokenType.CategoryOpening,
                TokenType.NewLine,
                TokenType.ValueSeparator,
                TokenType.ValueMarker,
            };

            _parser.IgnoredTokenTypes = new HashSet<TokenType>
            {
                TokenType.LineConcatenator,
            };
        }

        /// <summary>
        /// Parse values for current key
        /// </summary>
        protected void InitKeyValueParsing()
        {
            _parsingType = "key value parsing";
            ClearAllMyCallbacks();
            _parser.ValidTokenFound += ValidTokenFoundDuringKeyValueParsing;

            _parser.AllowedTokenTypes = new HashSet<TokenType>
            {
                TokenType.ValueSeparator,
                TokenType.Letter,
                TokenType.NewLine,
                TokenType.Space,
                TokenType.WhiteSpace,
                TokenType.InlineComment,
                TokenType.ValueMarker
            };

            _parser.IgnoredTokenTypes = new HashSet<TokenType>()
            {
                TokenType.LineConcatenator,
                TokenType.Equality
            };
        }

        protected void InitPureValueParsing()
        {
            _parsingType = "pure value parsing";
            ClearAllMyCallbacks();
            _parser.ValidTokenFound += ValidTokenFoundDuringPureValueParsing;

            _parser.AllowedTokenTypes = new HashSet<TokenType>
            {
                TokenType.Letter,
                TokenType.ValueMarker,
                TokenType.Space,
                TokenType.WhiteSpace,
                TokenType.ValueSeparator
            };

            _parser.IgnoredTokenTypes = new HashSet<TokenType>()
            {
                TokenType.InlineComment,
                TokenType.LineConcatenator,
                TokenType.Equality,
                TokenType.CategoryOpening,
                TokenType.CategoryClosing
            };
        }

        /// <summary>
        /// Parse comments
        /// </summary>
        protected void InitCommentParsing(Action previous)
        {
            _parsingType = "comment parsing";
            _previousParsing = previous;
            ClearAllMyCallbacks();
            _parser.ValidTokenFound += ValidTokenFoundDuringCommentParsing;

            _parser.AllowedTokenTypes = new HashSet<TokenType>
            {
                TokenType.NewLine,
            };

            _parser.IgnoredTokenTypes = new HashSet<TokenType>()
            {
                TokenType.Letter,
                TokenType.Space,
                TokenType.WhiteSpace,
                TokenType.ValueMarker,
                TokenType.ValueSeparator,
                TokenType.LineConcatenator,
                TokenType.Equality,
                TokenType.InlineComment,
                TokenType.CategoryOpening,
                TokenType.CategoryClosing,
            };
        }

        // Parsing value inside ""
        private void ValidTokenFoundDuringPureValueParsing(object sender, Token token)
        {
            switch (token.Type)
            {
                case TokenType.Letter:
                case TokenType.ValueSeparator:
                case TokenType.WhiteSpace:
                case TokenType.Space:
                    _keyTmpValue += token.Symbol;
                    break;
                case TokenType.ValueMarker:
                    ValueParsingComplete(true);
                    InitKeyValueParsing();
                    break;
                default:
                    throw new InvalidTokenException("Invalid tokenBase found during comment parsing: " + token.Symbol);
            }
        }

        //Parsing inline comment
        private void ValidTokenFoundDuringCommentParsing(object sender, Token token)
        {
            switch (token.Type)
            {
                case TokenType.NewLine:
                    _previousParsing();
                    break;
                default:
                    throw new InvalidTokenException("Invalid tokenBase found during comment parsing: " + token.Symbol);
            }
        }

        //Parsing top layer
        protected void ValidTokenFoundDuringMainParsing(object sender, Token token)
        {
            switch (token.Type)
            {
                case TokenType.InlineComment:
                    //go to next line, init comment parsing
                    InitCommentParsing(InitMainParsing);
                    break;
                case TokenType.CategoryOpening:
                    _currentCategory = new Category();
                    InitCategoryParsing();
                    break;
                default:
                    throw new InvalidTokenException("Invalid tokenBase found during parsing of the file: " + token.Symbol);
            }
        }

        //when parsing a category
        protected void ValidTokenFoundDuringCategoryParsing(object sender, Token token)
        {
            switch (token.Type)
            {
                case TokenType.CategoryClosing:
                    InitKeyIdParsing();
                    break;
                case TokenType.LineConcatenator:
                    if (this._parser.Position == (this._parser.Length - 1))
                    {
                        throw new InvalidTokenException(@"'\' are not allowed as the last token in a Category");
                    }
                    _currentCategory.Name += token.Symbol;
                    break;
                case TokenType.Letter:
                case TokenType.Space:
                    _currentCategory.Name += token.Symbol;
                    break;
                default:
                    throw new InvalidTokenException("Invalid tokenBase found during parsing of the file: " + token.Symbol);
            }
        }

        //when parsing a tokenBase id
        protected void ValidTokenFoundDuringKeyIdParsing(object sender, Token token)
        {
            switch (token.Type)
            {
                case TokenType.ValueMarker:
                    InitPureValueParsing();
                    break;
                case TokenType.ValueSeparator:
                    SerializeCurrentTmpValueAsAnonymousKey();
                    break;
                case TokenType.NewLine:
                    if (!string.IsNullOrEmpty(_keyTmpValue))
                        SerializeCurrentTmpValueAsAnonymousKey();
                    break;
                case TokenType.Equality:
                    // multiple EQ tokens in formula
                    if (!string.IsNullOrEmpty(_currentKey.Id))
                    {
                        throw new InvalidTokenException("Equality tokenBase detected, but not expected.");
                    }
                    KeyIdParsingCompleted();
                    InitKeyValueParsing();
                    break;
                case TokenType.WhiteSpace:
                case TokenType.Space:
                    //ignore spaces at the begining
                    if (!string.IsNullOrEmpty(_keyTmpValue))
                    {
                        _keyTmpValue += token.Symbol;
                    }
                    break;
                case TokenType.Letter:
                    _keyTmpValue += token.Symbol;
                    break;
                case TokenType.CategoryOpening:
                    KeyParsingComplete();
                    CategoryParsingComplete();
                    InitCategoryParsing();
                    break;
                case TokenType.InlineComment:
                    InitCommentParsing(InitKeyIdParsing);
                    break;
                default:
                    throw new InvalidTokenException("Invalid tokenBase found during parsing of the file: " + token.Symbol);
            }
        }

        //When parsing value
        protected void ValidTokenFoundDuringKeyValueParsing(object sender, Token token)
        {
            switch (token.Type)
            {
                case TokenType.ValueSeparator:
                    ValueParsingComplete();
                    break;
                case TokenType.Letter:
                    _keyTmpValue += token.Symbol;
                    break;
                case TokenType.NewLine:
                    ValueParsingComplete();
                    KeyParsingComplete();
                    InitKeyIdParsing();
                    break;
                case TokenType.WhiteSpace:
                    if (string.IsNullOrEmpty(_keyTmpValue))
                    {
                        ValueParsingComplete();
                    }
                    break;
                case TokenType.ValueMarker:
                    InitPureValueParsing();
                    break;
                case TokenType.InlineComment:
                    ValueParsingComplete();
                    KeyParsingComplete();
                    InitCommentParsing(InitKeyIdParsing);
                    break;
            }
        }

        protected void InvalidTokenFound(object sender, Token token)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Invalid tokenBase found during {_parsingType} parsing: ");
            builder.AppendLine($"Symbol: {token.Symbol}");
            builder.AppendLine($"Token type: {token.Type}");
            builder.AppendLine($"Allowed tokens: {string.Join(", ", _parser.AllowedTokenTypes.Select(t => t.ToString()))}");
            builder.AppendLine($"Ignored tokens: {string.Join(", ", _parser.IgnoredTokenTypes.Select(t => t.ToString()))}");
            throw new InvalidTokenException(builder.ToString());
        }

        protected void KeyParsingComplete()
        {
            if (_currentKey != null && _currentKey.KeyValues.Any())
            {
                _currentCategory.Keys.Add(_currentKey);
                _currentKey = null;
            }
        }

        protected void ValueParsingComplete(bool pure = false)
        {
            if (!string.IsNullOrEmpty(_keyTmpValue))
            {
                KeyValue keyValue;
                if (!pure)
                {
                    keyValue = new KeyValue
                    {
                        Value = _keyTmpValue
                    };
                }
                else
                {
                    keyValue = new PureValue
                    {
                        Value = _keyTmpValue
                    };
                }
                _currentKey.KeyValues.Add(keyValue);
                _keyTmpValue = null;
            }
        }

        protected void CategoryParsingComplete()
        {
            if (_currentCategory != null)
            {
                CategoryDiscovered?.Invoke(this, _currentCategory);
                _currentCategory = null;
            }
        }

        protected void SerializeCurrentTmpValueAsAnonymousKey()
        {
            //TODO Implement this
            var keyValue = new KeyValue
            {
                Value = _keyTmpValue
            };
            _currentKey.KeyValues.Add(keyValue);
            _keyTmpValue = null;
        }

        protected void KeyIdParsingCompleted()
        {
            // trim any leading or trailing whitespace
            _currentKey.Id = _keyTmpValue.Trim();
            _keyTmpValue = null;
        }

        private void ClearAllMyCallbacks()
        {
            _parser.ValidTokenFound -= ValidTokenFoundDuringMainParsing;
            _parser.ValidTokenFound -= ValidTokenFoundDuringCategoryParsing;
            _parser.ValidTokenFound -= ValidTokenFoundDuringKeyIdParsing;
            _parser.ValidTokenFound -= ValidTokenFoundDuringKeyValueParsing;
            _parser.ValidTokenFound -= ValidTokenFoundDuringCommentParsing;
            _parser.ValidTokenFound -= ValidTokenFoundDuringPureValueParsing;
        }
    }
}