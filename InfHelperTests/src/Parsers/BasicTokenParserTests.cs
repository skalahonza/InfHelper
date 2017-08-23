using System.Collections.Generic;
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
            var parser = new BasicTokenParser(new HashSet<TokenBase>
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<TokenBase>());
            string result = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.Parse(formula);

            Assert.AreEqual(formula, result);
        }

        [TestMethod()]
        public void IgnoredTokensTest()
        {
            string expression = "[TEST]";
            string formula = "  \n   " + expression + "   \n   ";
            var parser = new BasicTokenParser(new HashSet<TokenBase>
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<TokenBase>
            {
                new WhiteSpaceToken(),
                new NewLineToken()
            });

            string result = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.Parse(formula);

            Assert.AreEqual(expression, result);
        }

        [TestMethod()]
        public void AllowedTokensTest()
        {
            string formula = "[TE;ST] \\";
            var parser = new BasicTokenParser(new HashSet<TokenBase>
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<TokenBase>
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
            parser.Parse(formula);

            Assert.IsTrue(invalids.Contains(";") && invalids.Contains("\\"));
        }

        [TestMethod()]
        public void TokensWithSameSymbol()
        {
            string formula = "Test = test\\\ntest";
            var parser = new BasicTokenParser(new HashSet<TokenBase>
            {
                new LetterToken(),
                new EqualityToken(),
                new LineConcatenatorToken()
            }, new HashSet<TokenBase>
            {
                new WhiteSpaceToken(),
                new NewLineToken()
            });

            var tokens = new List<TokenType>();
            parser.ValidTokenFound += (sender, token) => tokens.Add(token.Type);
            parser.Parse(formula);

            Assert.IsTrue(tokens.Contains(TokenType.EQ) && tokens.Contains(TokenType.Letter) && tokens.Contains(TokenType.LineConcatenator));
        }

        [TestMethod()]
        public void TestOfAdaptability()
        {
            string formula = "Test = test\\\ntest";
            var parser = new BasicTokenParser(new HashSet<TokenBase>
            {
                new LetterToken(),
                new EqualityToken(),
            }, new HashSet<TokenBase>
            {
                new WhiteSpaceToken(),
            });

            string id = "";
            string key = "";

            void keyParsing(object sender, TokenBase token)
            {
                switch (token.Type)
                {
                    case TokenType.Letter:
                        id += token.Symbol;
                        break;
                    case TokenType.EQ:
                        parser.ValidTokenFound -= keyParsing;
                        parser.AllowedTokens = new HashSet<TokenBase>()
                        {
                            new LetterToken(),
                            new NewLineToken(),
                            new LineConcatenatorToken(),
                        };
                        parser.ValidTokenFound += valueParsing;
                        break;
                }
            }

            void valueParsing(object sender, TokenBase token)
            {
                switch (token.Type)
                {
                    case TokenType.Letter:
                        key += token.Symbol;
                        break;
                    case TokenType.LineConcatenator:
                        parser.IgnoredTokens.Add(new NewLineToken());                  
                        break;
                }
            }

            parser.ValidTokenFound += keyParsing;
            parser.Parse(formula);

            Assert.AreEqual(id,"Test");
            Assert.AreEqual(key,"testtest");
        }
    }
}