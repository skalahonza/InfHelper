using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Models.Tokens;
using InfHelper.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfHelperTests.Parsers
{
    [TestClass()]
    public class BasicTokenParserTests
    {
        [TestMethod()]
        public void TokenOrderTest()
        {
            string formula = "[TEST]";
            var parser = new BasicTokenParser(new HashSet<TokenType>
            {
                TokenType.CategoryOpening,
                TokenType.Letter,
                TokenType.CategoryClosing,
            }, new HashSet<TokenType>());
            TestTokenHandler validTokenHandler = new TestTokenHandler();
            parser.ValidTokenFound += validTokenHandler.EventHandler;
            parser.Parse(formula);

            Assert.AreEqual(formula, validTokenHandler.Result);
        }

        [TestMethod()]
        public void IgnoredTokensTest()
        {
            string expression = "[TEST]";
            string formula = "  \n   " + expression + "   \n   ";
            var parser = new BasicTokenParser(new HashSet<TokenType>
            {
                TokenType.CategoryOpening,
                TokenType.Letter,
                TokenType.CategoryClosing,
            }, new HashSet<TokenType>
            {
                TokenType.WhiteSpace,
                TokenType.NewLine
            });

            TestTokenHandler validTokenHandler = new TestTokenHandler();
            parser.ValidTokenFound += validTokenHandler.EventHandler;
            parser.Parse(formula);

            Assert.AreEqual(expression, validTokenHandler.Result);
        }

        [TestMethod()]
        public void AllowedTokensTest()
        {
            string formula = "[TE;ST] \\";
            var parser = new BasicTokenParser(new HashSet<TokenType>
            {
               TokenType.CategoryOpening,
               TokenType.Letter,
               TokenType.CategoryClosing,
            }, new HashSet<TokenType>
            {
                TokenType.WhiteSpace,
                TokenType.NewLine
            });

            TestTokenHandler invalidTokenHandler = new TestTokenHandler();
            parser.InvalidTokenFound += invalidTokenHandler.EventHandler;
            
            parser.Parse(formula);

            Assert.AreEqual("; \\", invalidTokenHandler.Result);
        }

        [TestMethod()]
        public void TokensWithSameSymbol()
        {
            string formula = "Test = test\\\ntest";
            var parser = new BasicTokenParser(new HashSet<TokenType>
            {
                TokenType.Letter,
                TokenType.Equality,
                TokenType.LineConcatenator
            }, new HashSet<TokenType>
            {
                TokenType.WhiteSpace,
                TokenType.NewLine
            });

            var missingTokens = new HashSet<TokenType> {TokenType.Equality, TokenType.Letter, TokenType.LineConcatenator};
            parser.ValidTokenFound += (sender, token) => missingTokens.Remove(token.Type);
            parser.Parse(formula);

            Assert.IsFalse(missingTokens.Any(), 
                "Missing expected Tokens: " + string.Join(", ", missingTokens.Select(a => a.ToString())));
        }

        [TestMethod()]
        public void TestOfAdaptability()
        {
            string formula = "Test = test\\\ntest";
            var parser = new BasicTokenParser(new HashSet<TokenType>
            {
                TokenType.Letter,
                TokenType.Equality,
            }, new HashSet<TokenType>
            {
                TokenType.WhiteSpace,
            });

            string id = "";
            string key = "";

            void KeyParsing(object sender, Token token)
            {
                switch (token.Type)
                {
                    case TokenType.Letter:
                        id += token.Symbol;
                        break;
                    case TokenType.Equality:
                        parser.ValidTokenFound -= KeyParsing;
                        parser.AllowedTokenTypes = new HashSet<TokenType>()
                        {
                            TokenType.Letter,
                            TokenType.NewLine,
                            TokenType.LineConcatenator,
                        };
                        parser.ValidTokenFound += ValueParsing;
                        break;
                }
            }

            void ValueParsing(object sender, Token token)
            {
                switch (token.Type)
                {
                    case TokenType.Letter:
                        key += token.Symbol;
                        break;
                    case TokenType.LineConcatenator:
                        parser.IgnoredTokenTypes.Add(TokenType.NewLine);                  
                        break;
                }
            }

            parser.ValidTokenFound += KeyParsing;
            parser.Parse(formula);

            Assert.AreEqual(id,"Test");
            Assert.AreEqual(key,"testtest");
        }
    }

    class TestTokenHandler
    {
        public EventHandler<Token> EventHandler => (sender, token) => Result += token.Symbol;

        public string Result { get; private set; }
    }
    
}