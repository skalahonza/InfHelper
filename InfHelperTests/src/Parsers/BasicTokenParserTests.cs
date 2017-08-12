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
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>()
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<IToken>());
            string result = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.ParseToken(formula);

            Assert.AreEqual(formula, result);
        }

        [TestMethod()]
        public void IgnoredTokensTest()
        {
            string expression = "[TEST]";
            string formula = "  \n   "+ expression +"   \n   ";
            var parser = new BasicTokenParser(BasicTokenParser.AllAvailableTokens, new HashSet<IToken>()
            {
                new CategoryOpeningToken(),
                new LetterToken(),
                new CategoryClosingToken(),
            }, new HashSet<IToken>()
            {
                new WhiteSpaceToken(),
                new NewLineToken()
            });

            string result = "";
            parser.ValidTokenFound += (sender, token) => result += token.Symbol;
            parser.ParseToken(formula);

            Assert.AreEqual(expression, result);
        }
    }
}