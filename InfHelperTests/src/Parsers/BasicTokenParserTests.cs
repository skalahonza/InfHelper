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
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<IToken>());
            string result = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.ParseFormula(formula);

            Assert.AreEqual(formula, result);
        }

        [TestMethod()]
        public void IgnoredTokensTest()
        {
            string expression = "[TEST]";
            string formula = "  \n   " + expression + "   \n   ";
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<IToken>
            {
                new WhiteSpaceToken(),
                new NewLineToken()
            });

            string result = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.ParseFormula(formula);

            Assert.AreEqual(expression, result);
        }

        [TestMethod()]
        public void AllowedTokensTest()
        {
            string formula = "[TE;ST] \\";
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<IToken>
            {
                new WhiteSpaceToken(),
                new NewLineToken()
            });

            string result = "";
            string invalids = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.InvalidTokenFound += (sender, token) =>
            {
                invalids += token.Symbol;
            };
            parser.ParseFormula(formula);

            Assert.IsTrue(invalids.Contains(";") && invalids.Contains("\\"));
        }

        [TestMethod()]
        public void TokensWithSameSymbol()
        {
            string formula = "Test = test\\\ntest";
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>
            {
                new LetterToken(),
                new EqualityToken(),
                new LineConcatenatorToken()
            }, new HashSet<IToken>
            {
                new WhiteSpaceToken(),
                new NewLineToken()
            });

            var tokens = new List<TokenType>();
            parser.ValidTokenFound += (sender, token) => tokens.Add(token.Type);
            parser.ParseFormula(formula);

            Assert.IsTrue(tokens.Contains(TokenType.EQ) && tokens.Contains(TokenType.Letter) && tokens.Contains(TokenType.LineConcatenator));
        }

        [TestMethod()]
        public void TestOfAdaptability()
        {
            string formula = "Test = test\\\ntest";
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>
            {
                new LetterToken(),
                new EqualityToken(),
            }, new HashSet<IToken>
            {
                new WhiteSpaceToken(),
            });

            string id = "";
            string key = "";

            void keyParsing(object sender, IToken token)
            {
                switch (token.Type)
                {
                    case TokenType.Letter:
                        id += token.Symbol;
                        break;
                    case TokenType.EQ:
                        parser.ValidTokenFound -= keyParsing;
                        parser.AllowedTokens = new HashSet<IToken>()
                        {
                            new LetterToken(),
                            new NewLineToken(),
                            new LineConcatenatorToken(),
                        };
                        parser.ValidTokenFound += valueParsing;
                        break;
                }
            }

            void valueParsing(object sender, IToken token)
            {
                switch (token.Type)
                {
                    case TokenType.Letter:
                        key += token.Symbol;
                        break;
                    case TokenType.LineConcatenator:
                        parser.IgnoredTokens = parser.IgnoredTokens.Concat(new[] {new NewLineToken()});                    
                        break;
                }
            }

            parser.ValidTokenFound += keyParsing;
            parser.ParseFormula(formula);

            Assert.AreEqual(id,"Test");
            Assert.AreEqual(key,"testtest");
        }
    }
}